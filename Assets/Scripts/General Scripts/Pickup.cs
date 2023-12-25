using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    public Transform itemPos;
    public float range = 10f;
    public GameObject pickupHandImage; // Reference to the UI Image

    GameObject currentItem;
    GameObject item;
    Collider playerCollider; // Reference to the player's collider

    bool canGrab;

    private void Start()
    {
        playerCollider = GetComponent<Collider>();
        pickupHandImage.SetActive(false); // Initially hide the pickup hand image
    }

    private void Update()
    {
        CheckItems();

        if (canGrab)
        {
            pickupHandImage.SetActive(true); // Show the pickup hand image

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentItem != null)
                    Drop();

                PickupItem();
            }
        }
        else
        {
            pickupHandImage.SetActive(false); // Hide the pickup hand image
        }

        if (currentItem != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
                Drop();
        }
    }

    private void CheckItems()
    {
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("IgnorePlayer"); // Layer mask for items to ignore player collisions

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            if (hit.transform.tag == "CanGrab" && IsObjectInView(hit.point))
            {
                canGrab = true;
                item = hit.transform.gameObject;
            }
            else
            {
                canGrab = false;
            }
        }
        else
        {
            canGrab = false;
        }
    }

    private bool IsObjectInView(Vector3 objectPosition)
    {
        Vector3 toObject = objectPosition - Camera.main.transform.position;
        float angle = Vector3.Angle(Camera.main.transform.forward, toObject);

        // You can adjust the threshold angle based on your preference
        float maxViewAngle = 60f;

        return angle <= maxViewAngle;
    }

    private void PickupItem()
    {
        currentItem = item;

        // Ignore collisions between player and item
        Physics.IgnoreCollision(playerCollider, currentItem.GetComponent<Collider>(), true);

        currentItem.transform.position = itemPos.position;
        currentItem.transform.parent = itemPos;
        currentItem.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        currentItem.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Drop()
    {
        // Re-enable collisions between player and previously held item
        if (currentItem != null)
        {
            Physics.IgnoreCollision(playerCollider, currentItem.GetComponent<Collider>(), false);
            currentItem.layer = LayerMask.NameToLayer("Default");
            currentItem.transform.parent = null;
            currentItem.GetComponent<Rigidbody>().isKinematic = false;
            currentItem = null;
        }
    }
}
