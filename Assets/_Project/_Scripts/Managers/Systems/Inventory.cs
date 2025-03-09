using System;
using System.Collections.Generic;
using UnityEngine;

/****************************************************
 *                  INVENTORY CLASS                *
 ****************************************************
 * Description: This class manages the inventory   *
 * system in the game. It stores a list of items   *
 * and provides functionality to add new items to  *
 * the inventory. It also triggers an event when   *
 * the item list changes.                          *
 *                                                 *
 * Features:                                       *
 * - Stores and manages a list of items            *
 * - Allows adding items to the inventory          *
 * - Triggers an event when the item list changes  *
 * - Displays item count during initialization     *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class Inventory
    {
        public event EventHandler OnItemListChanged;
        private List<Item> _itemList;
        private Action<Item> useItem;

        // Inventory displays sprites contained in ItemAssets.cs
        public Inventory(Action<Item> useItem)
        {
            this.useItem = useItem;
            _itemList = new List<Item>();

            //-------------------Manually Add Inventory Items--------------------
            //-------------------------------------------------------------------
            //AddItem(new Item { itemType = Item.ItemType.Medkit, amount = 1 });
            //AddItem(new Item { itemType = Item.ItemType.Key, amount = 1 });
            //AddItem(new Item { itemType = Item.ItemType.Health, amount = 1 });
            //AddItem(new Item { itemType = Item.ItemType.Ammo, amount = 1 });
            //-------------------------------------------------------------------
            //-------------------------------------------------------------------
            Debug.Log(_itemList.Count);
        }

        public void AddItem(Item item)
        {
            Debug.Log($"Adding item: {item.itemType}, Amount: {item.amount}, Stackable: {item.IsStackable()}");
            if (item.IsStackable())
            {
                bool itemAlreadyAdded = false;
                foreach (Item inventoryItem in _itemList)
                {
                    if (inventoryItem.itemType == item.itemType)
                    {
                        Debug.Log(
                            $"Stacking item: {inventoryItem.itemType}. Current amount: {inventoryItem.amount}, Adding: {item.amount}");
                        inventoryItem.amount += item.amount;
                        itemAlreadyAdded = true;
                    }
                }

                if (!itemAlreadyAdded)
                {
                    Debug.Log($"New stackable item added to inventory: {item.itemType}");
                    _itemList.Add(item);
                }
            }
            else
            {
                _itemList.Add(item);
            }

            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem(Item item)
        {
            Debug.Log($"Removing item: {item.itemType}, Amount: {item.amount}, Stackable: {item.IsStackable()}");
            if (item.IsStackable())
            {
                Item itemInInventory = null;
                foreach (Item inventoryItem in _itemList)
                {
                    if (inventoryItem.itemType == item.itemType)
                    {
                        Debug.Log(
                            $"Stacking item: {inventoryItem.itemType}. Current amount: {inventoryItem.amount}, Adding: {item.amount}");
                        inventoryItem.amount -= item.amount;
                        itemInInventory = inventoryItem;
                    }
                }

                if (itemInInventory != null && itemInInventory.amount <= 0)
                {
                    Debug.Log($"New stackable item removed from inventory: {item.itemType}");
                    _itemList.Remove(itemInInventory);
                }
            }
            else
            {
                _itemList.Remove(item);
            }

            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UseItem(Item item)
        {
            useItem(item);
        }

        public List<Item> GetItemList()
        {
            return _itemList;
        }
    }
}