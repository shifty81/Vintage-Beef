using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace VintageBeef
{
    /// <summary>
    /// Player inventory system for storing gathered resources and items
    /// Supports stacking and slot management
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        [Header("Inventory Settings")]
        [SerializeField] private int maxSlots = 30;
        [SerializeField] private int maxStackSize = 99;

        private List<InventorySlot> slots = new List<InventorySlot>();

        private void Awake()
        {
            InitializeInventory();
        }

        private void InitializeInventory()
        {
            for (int i = 0; i < maxSlots; i++)
            {
                slots.Add(new InventorySlot());
            }
            Debug.Log($"Inventory initialized with {maxSlots} slots.");
        }

        /// <summary>
        /// Add a resource to the inventory
        /// </summary>
        public bool AddResource(World.ResourceItem resource)
        {
            return AddItem(resource.name, resource.amount, resource.type);
        }

        /// <summary>
        /// Add an item to the inventory
        /// </summary>
        public bool AddItem(string itemName, int amount, string itemType)
        {
            // Try to stack with existing items first
            foreach (InventorySlot slot in slots)
            {
                if (slot.isEmpty) continue;
                
                if (slot.itemName == itemName && slot.quantity < maxStackSize)
                {
                    int spaceInStack = maxStackSize - slot.quantity;
                    int amountToAdd = Mathf.Min(spaceInStack, amount);
                    
                    slot.quantity += amountToAdd;
                    amount -= amountToAdd;

                    Debug.Log($"Added {amountToAdd} {itemName} to existing stack. Total: {slot.quantity}");

                    if (amount <= 0)
                    {
                        return true;
                    }
                }
            }

            // Create new stacks for remaining items
            while (amount > 0)
            {
                InventorySlot emptySlot = slots.FirstOrDefault(s => s.isEmpty);
                
                if (emptySlot == null)
                {
                    Debug.LogWarning("Inventory full! Cannot add more items.");
                    return false;
                }

                int amountToAdd = Mathf.Min(maxStackSize, amount);
                emptySlot.itemName = itemName;
                emptySlot.itemType = itemType;
                emptySlot.quantity = amountToAdd;
                emptySlot.isEmpty = false;

                Debug.Log($"Added {amountToAdd} {itemName} to new slot. Total in inventory: {GetItemCount(itemName)}");

                amount -= amountToAdd;
            }

            return true;
        }

        /// <summary>
        /// Remove an item from the inventory
        /// </summary>
        public bool RemoveItem(string itemName, int amount)
        {
            int totalAvailable = GetItemCount(itemName);
            
            if (totalAvailable < amount)
            {
                Debug.LogWarning($"Not enough {itemName}. Have: {totalAvailable}, Need: {amount}");
                return false;
            }

            int remainingToRemove = amount;

            for (int i = slots.Count - 1; i >= 0; i--)
            {
                InventorySlot slot = slots[i];
                
                if (slot.isEmpty || slot.itemName != itemName) continue;

                if (slot.quantity <= remainingToRemove)
                {
                    remainingToRemove -= slot.quantity;
                    slot.Clear();
                }
                else
                {
                    slot.quantity -= remainingToRemove;
                    remainingToRemove = 0;
                }

                if (remainingToRemove <= 0)
                {
                    break;
                }
            }

            Debug.Log($"Removed {amount} {itemName}. Remaining: {GetItemCount(itemName)}");
            return true;
        }

        /// <summary>
        /// Get total count of a specific item
        /// </summary>
        public int GetItemCount(string itemName)
        {
            int count = 0;
            foreach (InventorySlot slot in slots)
            {
                if (!slot.isEmpty && slot.itemName == itemName)
                {
                    count += slot.quantity;
                }
            }
            return count;
        }

        /// <summary>
        /// Check if inventory has enough of an item
        /// </summary>
        public bool HasItem(string itemName, int amount)
        {
            return GetItemCount(itemName) >= amount;
        }

        /// <summary>
        /// Get all items in inventory
        /// </summary>
        public List<InventorySlot> GetAllItems()
        {
            return slots.Where(s => !s.isEmpty).ToList();
        }

        /// <summary>
        /// Get all items of a specific type
        /// </summary>
        public List<InventorySlot> GetItemsByType(string type)
        {
            return slots.Where(s => !s.isEmpty && s.itemType == type).ToList();
        }

        /// <summary>
        /// Clear entire inventory
        /// </summary>
        public void ClearInventory()
        {
            foreach (InventorySlot slot in slots)
            {
                slot.Clear();
            }
            Debug.Log("Inventory cleared.");
        }

        /// <summary>
        /// Get number of empty slots
        /// </summary>
        public int GetEmptySlotCount()
        {
            return slots.Count(s => s.isEmpty);
        }

        /// <summary>
        /// Get number of used slots
        /// </summary>
        public int GetUsedSlotCount()
        {
            return slots.Count(s => !s.isEmpty);
        }

        /// <summary>
        /// Print inventory contents (for debugging)
        /// </summary>
        public void PrintInventory()
        {
            Debug.Log("=== INVENTORY ===");
            foreach (InventorySlot slot in slots.Where(s => !s.isEmpty))
            {
                Debug.Log($"- {slot.itemName} x{slot.quantity} ({slot.itemType})");
            }
            Debug.Log($"Empty slots: {GetEmptySlotCount()}/{maxSlots}");
            Debug.Log("================");
        }
    }

    /// <summary>
    /// Represents a single inventory slot
    /// </summary>
    [System.Serializable]
    public class InventorySlot
    {
        public bool isEmpty = true;
        public string itemName = "";
        public string itemType = "";
        public int quantity = 0;

        public void Clear()
        {
            isEmpty = true;
            itemName = "";
            itemType = "";
            quantity = 0;
        }
    }
}
