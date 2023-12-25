using UnityEngine;

public class Battery : MonoBehaviour
{
    public float batteryValue = 40f; 

    private Camera playerCamera;

    private void Start()
    {
        playerCamera = Camera.main; 
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {

            if (IsPlayerLookingAtBattery() && IsFlashlightHeld())
            {
                CollectBattery();
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