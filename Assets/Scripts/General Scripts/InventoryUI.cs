using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class InventoryItem
{
    public GameObject itemObject;
    public Sprite itemSprite;
}

public class InventoryUI : MonoBehaviour
{
    public Image[] inventorySlots;
    public Sprite emptySlotSprite; // Placeholder for an empty slot sprite
    public Sprite defaultItemSprite; // Placeholder for the default item sprite (when no specific sprite is assigned)

    public List<InventoryItem> inventoryItems = new List<InventoryItem>();

    private void Start()
    {
        // Initialize the inventory slots with empty sprites
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].sprite = GetEmptySlotSprite();
        }
    }

    public void UpdateInventory(List<GameObject> inventory)
    {
        // Update the inventory UI based on the items in the player's inventory
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventory.Count)
            {
                Image slotImage = inventorySlots[i];
                GameObject item = inventory[i];
                Sprite itemSprite = GetItemSprite(item, i); // Pass the item's index to the GetItemSprite method

                if (itemSprite != null)
                {
                    slotImage.sprite = itemSprite;
                }
                else
                {
                    // If the item doesn't have a specific sprite, set it to the default item sprite
                    slotImage.sprite = defaultItemSprite;
                }
            }
            else
            {
                // If the slot index is beyond the range of the inventory, set it to an empty slot sprite
                inventorySlots[i].sprite = GetEmptySlotSprite();
            }
        }
    }

    public void UpdateInventorySlot(int slotIndex, Sprite sprite)
    {
        if (slotIndex >= 0 && slotIndex < inventorySlots.Length)
        {
            inventorySlots[slotIndex].sprite = sprite;
        }
    }

    public Sprite GetEmptySlotSprite()
    {
        // Return the sprite for an empty inventory slot
        return emptySlotSprite;
    }

    private Sprite GetItemSprite(GameObject item, int itemIndex)
    {
        // Example: You might want to have different sprites for items based on their index
        // Modify this method according to your specific requirements
        foreach (var inventoryItem in inventoryItems)
        {
            if (inventoryItem.itemObject == item)
            {
                return inventoryItem.itemSprite;
            }
        }

        // Return null for items with a tag other than "CanGrab" or if no specific sprite is assigned
        return null;
    }
}
