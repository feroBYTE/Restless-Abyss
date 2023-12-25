using UnityEngine;

public class FlashlightCollisionController : MonoBehaviour
{
    private bool isInItemPos = false;
    private Collider flashlightCollider;

    private void Start()
    {
        flashlightCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("itemPos"))
        {
            // Flashlight is inside "itemPos", disable the collider
            flashlightCollider.enabled = false;
            isInItemPos = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("itemPos"))
        {
            // Flashlight is no longer in "itemPos", enable the collider
            flashlightCollider.enabled = true;
            isInItemPos = false;
        }
    }

    // You might want to add additional logic here to handle cases where the flashlight is disabled or moved elsewhere.
    // For example, if the player drops the flashlight, you may want to re-enable the collider.

    private void Update()
    {
        // Example: Check if the flashlight is disabled or moved elsewhere
        if (!isInItemPos && flashlightCollider.enabled == false)
        {
            flashlightCollider.enabled = true; // Enable the collider
        }
    }
}
