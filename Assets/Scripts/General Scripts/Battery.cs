using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    public float batteryValue = 40f;
    public AudioClip collectSound;
    private Camera playerCamera;
    public Image pickupHandImage;
    private bool isCollected = false;

    private void Start()
    {
        playerCamera = Camera.main;

        if (pickupHandImage == null)
        {
            Debug.LogError("Pickup Hand Image not assigned. Assign the UI image in the Inspector.");
        }
        else
        {
            HidePickupHandImage();
        }
    }

    private void Update()
    {
        if (isCollected)
        {
            // Battery has been collected, stop further updates
            return;
        }

        // Check if the player is looking at the battery and show pickup hand image
        if (IsPlayerLookingAtBattery())
        {
            ShowPickupHandImage();

            // Check for player consuming the battery
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Flashlight flashlight = FindObjectOfType<Flashlight>();

                // Check if the flashlight exists and its battery level is below 100%
                if (flashlight != null && flashlight.IsInItemPos() && flashlight.GetBatteryLevel() < 100f)
                {
                    CollectBattery();
                }

                // Hide the pickup hand image after collecting the battery
                HidePickupHandImage();
            }
        }
        else
        {
            HidePickupHandImage();
        }
    }

    private void ShowPickupHandImage()
    {
        if (pickupHandImage != null)
        {
            pickupHandImage.gameObject.SetActive(true);
        }
    }

    private void HidePickupHandImage()
    {
        if (pickupHandImage != null)
        {
            pickupHandImage.gameObject.SetActive(false);
        }
    }

    private bool IsPlayerLookingAtBattery()
    {
        if (playerCamera != null)
        {
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private void CollectBattery()
    {
        Flashlight flashlight = FindObjectOfType<Flashlight>();
        if (flashlight != null)
        {
            flashlight.SetNearbyBattery(this);

            // Play the collect battery sound
            if (flashlight.additionalAudioSource != null && collectSound != null)
            {
                flashlight.additionalAudioSource.PlayOneShot(collectSound);
            }
        }

        // Mark the battery as collected
        isCollected = true;

        // Destroy the battery after a short delay (adjust the delay as needed)
        Destroy(gameObject, 0.1f);
    }
}
