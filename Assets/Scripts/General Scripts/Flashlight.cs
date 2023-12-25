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
    private float batteryDrainRate = 50f;

    private void Start()
    {
        flashlightLight = GetComponentInChildren<Light>();
        audioSource = GetComponent<AudioSource>();

        flashlightLight.enabled = false;
        UpdateBatteryUI();

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
    }

    public void SetNearbyBattery(Battery battery)
    {
        // Set the nearby battery
        nearbyBattery = battery;

        // You can add additional logic here if needed

        // Example: Automatically collect the battery when nearby
        CollectBattery();
    }
    public void RechargeBattery(float amount)
    {
        // Increase the battery level by the specified amount
        batteryLevel = Mathf.Clamp(batteryLevel + amount, 0f, 100f);

        // Update the battery UI
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

    private void CollectBattery()
    {
        if (nearbyBattery != null)
        {
            // Add batteryValue to the flashlight's battery level
            RechargeBattery(nearbyBattery.batteryValue);

            // Play collection sound or perform other collection actions
            Debug.Log("Collecting battery. Battery value: " + nearbyBattery.batteryValue);

            // Remove the battery object from the scene
            Destroy(nearbyBattery.gameObject);

            // Reset nearbyBattery after collecting
            nearbyBattery = null;
        }
    }


    private void UpdateBatteryUI()
    {
        if (batteryImage != null && batterySprites != null && batterySprites.Length >= 6 && IsInItemPos())
        {
            int spriteIndex = Mathf.Clamp(Mathf.FloorToInt(batteryLevel / 20f), 0, 5);

            batteryImage.sprite = batterySprites[spriteIndex];
        }
    }

    private void SetBatteryUIVisibility(bool isVisible)
    {
        if (batteryImage != null)
        {
            batteryImage.gameObject.SetActive(isVisible);
        }
    }
}
