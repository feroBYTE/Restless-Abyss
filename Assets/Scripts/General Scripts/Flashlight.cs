using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.F;
    public KeyCode collectKey = KeyCode.E; // Add this line for the collect key

    private Battery nearbyBattery;
    public AudioClip toggleOnSound;
    public AudioClip toggleOffSound;
    public AudioSource additionalAudioSource;

    public Image batteryImage;
    public Sprite[] batterySprites;

    private Light flashlightLight;
    private AudioSource audioSource;
    private float batteryLevel = 100f;
    private float batteryDrainRate = 100f / 180f;
    // private float batteryDrainRate = 15f;
    private Collider flashlightCollider;
    private void Start()
    {
        flashlightLight = GetComponentInChildren<Light>();
        audioSource = GetComponent<AudioSource>();

        flashlightLight.enabled = false;
        UpdateBatteryUI();
        flashlightCollider = GetComponent<Collider>();
        flashlightCollider.enabled = true;
        SetBatteryUIVisibility(false);
    }

    private void Update()
    {
        if (IsInItemPos())
        {
            SetBatteryUIVisibility(true);

            if (Input.GetKeyDown(toggleKey))
            {
                ToggleFlashlight();
            }

            // Check for battery collection key press
            if (Input.GetKeyDown(collectKey) && nearbyBattery != null)
            {
                CollectBattery();
            }

            // Drain the battery over time
            if (flashlightLight.enabled && batteryLevel > 0f)
            {
                batteryLevel -= batteryDrainRate * Time.deltaTime;
                UpdateBatteryUI();
            }
            else if (flashlightLight.enabled)
            {
                flashlightLight.enabled = false;
                UpdateBatteryUI();
            }
        }
        else
        {
            SetBatteryUIVisibility(false);
        }

        if (IsInItemPos())
        {
            flashlightCollider.enabled = false; // Disable the collider
            SetBatteryUIVisibility(true);
        }
        else
        {
            flashlightCollider.enabled = true; // Enable the collider
            SetBatteryUIVisibility(false);
        }
    }

    public void SetNearbyBattery(Battery battery)
    {
        nearbyBattery = battery;
        CollectBattery();
    }
    public void RechargeBattery(float amount)
    {
        // Increase the battery level by the specified amount
        batteryLevel = Mathf.Clamp(batteryLevel + amount, 0f, 100f);

        UpdateBatteryUI();
    }
    public bool IsInItemPos()
    {
        return transform.parent != null && transform.parent.name == "itemPos";
    }

    private void ToggleFlashlight()
    {
        if (batteryLevel > 0f)
        {
            flashlightLight.enabled = !flashlightLight.enabled;

            if (audioSource != null)
            {
                if (flashlightLight.enabled && toggleOnSound != null)
                {
                    audioSource.PlayOneShot(toggleOnSound);
                }
                else if (!flashlightLight.enabled && toggleOffSound != null)
                {
                    audioSource.PlayOneShot(toggleOffSound);
                }
            }

            if (additionalAudioSource != null && toggleOnSound != null && toggleOffSound != null)
            {
                additionalAudioSource.PlayOneShot(flashlightLight.enabled ? toggleOnSound : toggleOffSound);
            }

            UpdateBatteryUI();
        }
    }
    public float GetBatteryLevel()
    {
        return batteryLevel;
    }

    private void CollectBattery()
    {
        if (nearbyBattery != null)
        {

            RechargeBattery(nearbyBattery.batteryValue);
            Destroy(nearbyBattery.gameObject);
            nearbyBattery = null;
        }
    }

    private void UpdateBatteryUI()
    {
        if (batteryImage != null && batterySprites != null && batterySprites.Length >= 6 && IsInItemPos())
        {
            int spriteIndex = 0;

            if (batteryLevel >= 90f)
            {
                spriteIndex = 0; // 100%
            }
            else if (batteryLevel >= 70f)
            {
                spriteIndex = 1; // 80%
            }
            else if (batteryLevel >= 50f)
            {
                spriteIndex = 2; // 60%
            }
            else if (batteryLevel >= 30f)
            {
                spriteIndex = 3; // 40%
            }
            else if (batteryLevel >= 10f)
            {
                spriteIndex = 4; // 20%
            }
            else
            {
                spriteIndex = 5; // 0%
            }

            // Check if the flashlight is the active item in the inventory
            if (IsFlashlightActive())
            {
                batteryImage.sprite = batterySprites[spriteIndex];
                batteryImage.gameObject.SetActive(true);
            }
            else
            {
                batteryImage.gameObject.SetActive(false);
            }
        }
    }

    // Add the following method to check if the flashlight is the active item
    private bool IsFlashlightActive()
    {
        Pickup pickupScript = GetComponentInParent<Pickup>();
        GameObject currentItem = pickupScript != null ? pickupScript.GetCurrentItem() : null;
        return currentItem != null && currentItem == gameObject;
    }

    public void SetBatteryUIVisibility(bool isVisible)
    {
        if (batteryImage != null)
        {
            batteryImage.gameObject.SetActive(isVisible);
        }
    }

    public void HideBatteryUI()
    {
        SetBatteryUIVisibility(false);
    }
}
