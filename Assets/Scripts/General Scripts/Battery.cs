using UnityEngine;

public class Battery : MonoBehaviour
{
    public float batteryValue = 40f;
    public AudioClip collectSound;
    private Camera playerCamera;

    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Flashlight flashlight = FindObjectOfType<Flashlight>();

            // Check if the flashlight exists and its battery level is below 100%
            if (flashlight != null && flashlight.IsInItemPos() && flashlight.GetBatteryLevel() < 100f)
            {
                if (IsPlayerLookingAtBattery() && flashlight.IsInItemPos())
                {
                    CollectBattery();
                }
            }
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

    private bool IsFlashlightHeld()
    {

        Flashlight flashlight = FindObjectOfType<Flashlight>();
        return flashlight != null && flashlight.IsInItemPos();
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

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        }
    }

    private void PlayCollectionSound()
    {

    }
}