using System;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Transform itemPos;
    public float range = 10f;
    public GameObject pickupHandImage; // Reference to the UI Image
    public AudioClip pickupSound;
    private AudioSource audioSource;
    private Collider playerCollider; // Reference to the player's collider
    private List<GameObject> inventory = new List<GameObject>(); // List to store the inventory items
    public InventoryUI inventoryUI;
    private bool canGrab;
    private int currentItemIndex = -1; // Index of the currently active item in the inventory

    // Define an event for notifying when the active item changes
    public event Action<int> OnActiveItemChanged;

    private Sprite flashlightSprite;

    private void Start()
    {
        playerCollider = GetComponent<Collider>();
        pickupHandImage.SetActive(false); // Initially hide the pickup hand image

        // Add the following lines to initialize the AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = pickupSound;
        inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI == null)
        {
            Debug.LogError("InventoryUI component not found. Make sure it's in the scene.");
        }

        // Corrected reference to sprites using the inventoryItems list
        if (inventoryUI.inventoryItems.Count > 0)
        {
            flashlightSprite = inventoryUI.inventoryItems[0].itemSprite;
        }
        else
        {
            Debug.LogError("No sprites assigned in the InventoryUI. Assign sprites in the Inspector.");
        }
    }


    private void Update()
    {
        CheckItems();
        UpdateInventoryUIHighlight();
        if (CanGrab())
        {
            pickupHandImage.SetActive(true); // Show the pickup hand image

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (inventory.Count < 3) // Limit inventory to 3 items
                {
                    PickupItem();
                    // Play the pickup sound when an item is picked up
                    if (pickupSound != null)
                        audioSource.Play();
                }
            }
        }
        else
        {
            pickupHandImage.SetActive(false);
            HideBatteryUI();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }

        // Check for number keys to swap between items
        if (Input.GetKeyDown(KeyCode.Alpha1) && inventory.Count >= 1)
        {
            SetCurrentItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && inventory.Count >= 2)
        {
            SetCurrentItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && inventory.Count >= 3)
        {
            SetCurrentItem(2);
        }
    }

    private void UpdateInventoryUIHighlight()
    {
        // Highlight the current active item in the inventory UI if there are items
        if (inventory.Count > 0)
        {
            inventoryUI.HighlightInventorySlot(currentItemIndex);
        }
        else
        {
            // Reset the highlight effect if there are no items in the inventory
            inventoryUI.HighlightInventorySlot(-1);
        }
    }

    private void HideBatteryUI()
    {
        Flashlight flashlight = GetActiveFlashlight();
        if (flashlight != null && inventory.Count > 0) // Check if there is an active item
        {
            flashlight.HideBatteryUI();
        }
    }


    private Flashlight GetActiveFlashlight()
    {
        if (currentItemIndex >= 0 && currentItemIndex < inventory.Count)
        {
            GameObject activeItem = inventory[currentItemIndex];
            Flashlight flashlight = activeItem.GetComponent<Flashlight>();
            return flashlight;
        }

        return null;
    }
    private void CheckItems()
    {
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("IgnorePlayer"); // Layer mask for items to ignore player collisions

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            if (hit.transform.CompareTag("CanGrab") && IsObjectInView(hit.point))
            {
                canGrab = true;
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

        float maxViewAngle = 60f;

        return angle <= maxViewAngle;
    }

    private bool CanGrab()
    {
        return canGrab;
    }

    private void PickupItem()
    {
        GameObject item = canGrab ? HitObject() : null;
        if (item != null)
        {
            if (inventory.Count < 3) // Limit inventory to 3 items
            {
                inventory.Add(item);
                SetItemsVisibility();
                SetCurrentItem(inventory.Count - 1); // Set the newly picked item as the current item
                item.transform.position = itemPos.position;
                item.transform.parent = itemPos;
                item.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                item.GetComponent<Rigidbody>().isKinematic = true;

                // Trigger the OnActiveItemChanged event
                OnActiveItemChanged?.Invoke(currentItemIndex);

                // Hide the battery UI for all items
                HideAllBatteryUI();

                // Ensure that the battery UI is correctly set when picking up the flashlight
                SetBatteryUIVisibilityForActiveItem();

                // Play the pickup sound when an item is picked up
                if (pickupSound != null)
                    audioSource.Play();

                // Hide the pickup hand image if the inventory is full
                if (inventory.Count >= 3)
                {
                    pickupHandImage.SetActive(false);
                }

                // Update the inventory UI in the InventoryUI component
                if (inventoryUI != null)
                {
                    inventoryUI.UpdateInventory(inventory);
                }
            }
        }
    }

    private void HideAllBatteryUI()
    {
        foreach (var item in inventory)
        {
            Flashlight flashlight = item.GetComponent<Flashlight>();
            if (flashlight != null)
            {
                flashlight.HideBatteryUI();
            }
        }
    }
    private void SetBatteryUIVisibilityForActiveItem()
    {
        Flashlight flashlight = GetActiveFlashlight();
        if (flashlight != null)
        {
            flashlight.SetBatteryUIVisibility(true);
        }
    }

    public GameObject GetCurrentItem()
    {
        if (currentItemIndex >= 0 && currentItemIndex < inventory.Count)
        {
            return inventory[currentItemIndex];
        }

        return null;
    }

    private void Drop()
    {
        if (currentItemIndex >= 0 && currentItemIndex < inventory.Count)
        {
            GameObject item = inventory[currentItemIndex];

            // Set the inventory slot sprite to the empty slot sprite
            inventoryUI.UpdateInventorySlot(currentItemIndex, inventoryUI.GetEmptySlotSprite());

            inventory.RemoveAt(currentItemIndex);

            // Re-enable collisions between player and the dropped item
            Physics.IgnoreCollision(playerCollider, item.GetComponent<Collider>(), false);
            item.layer = LayerMask.NameToLayer("Default");
            item.transform.parent = null;
            item.GetComponent<Rigidbody>().isKinematic = false;
            SetItemsVisibility();

            // Trigger the OnActiveItemChanged event
            OnActiveItemChanged?.Invoke(currentItemIndex);

            // Update the inventory UI in the InventoryUI component
            inventoryUI.UpdateInventory(inventory);

            // Ensure that the battery UI is correctly set when dropping the flashlight
            HideBatteryUI();
        }
    }



    private GameObject HitObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            return hit.transform.gameObject;
        }
        return null;
    }

    private void SetItemsVisibility()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].SetActive(i == currentItemIndex);
        }
    }

    private void SetCurrentItem(int index)
    {
        currentItemIndex = index;
        SetItemsVisibility();
    }
}
