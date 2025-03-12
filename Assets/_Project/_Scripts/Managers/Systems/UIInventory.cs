using _Project._Scripts.Units.Player;
using _Project._Scripts.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

/****************************************************
 *               UI INVENTORY SYSTEM                *
 ****************************************************
 * Description: This system manages the inventory UI *
 * in the game. It is responsible for displaying,   *
 * updating, and organizing inventory items using   *
 * Unity's UI system. The inventory panel updates  *
 * dynamically when items are added or removed.     *
 *                                                  *
 * Features:                                        *
 * - Dynamically updates inventory slots            *
 * - Listens for inventory changes                  *
 * - Handles missing components with error checks   *
 * - Supports customizable grid layouts             *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class UIInventory : MonoBehaviour
    {
        private Inventory _inventory;

        [FormerlySerializedAs("_itemSlotContainer")] [SerializeField]
        private Transform itemSlotContainer;

        [FormerlySerializedAs("_itemSlotTemplate")] [SerializeField]
        private Transform itemSlotTemplate;

        private Player _player;

        private void Start()
        {
            itemSlotContainer = transform.Find("Canvas/ItemSlotContainers");
            itemSlotTemplate = transform.Find("Canvas/ItemSlotContainers/ItemSlotTemplates");
            ValidateUIElements();
        }

        public void SetPlayer(Player player)
        {
            _player = player;
        }
        public void SetInventory(Inventory inventory)
        {
            if (inventory == null)
            {
                Debug.LogError("UIInventory: Inventory is null! Cannot set it.");
                return;
            }

            Debug.Log("UIInventory: Inventory successfully set.");
            _inventory = inventory;
            inventory.OnItemListChanged += Inventory_OnItemListChanged;
            RefreshInventoryItems();
        }

        private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
        {
            RefreshInventoryItems();
        }

        private void RefreshInventoryItems()
        {
            if (!ValidateInventory()) return;

            // Clear existing slots before adding new ones
            foreach (Transform child in itemSlotContainer)
            {
                if (child == itemSlotTemplate) continue;
                Destroy(child.gameObject);
            }

            var x = 0;
            var y = 0;
            const float itemSlotCellSize = 40.0f;
            const int maxColumns = 4; // Adjust as needed for UI layout
            foreach (var item in _inventory.GetItemList())
            {
                var itemSlotRectTransform =
                    Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                
                // Use item
                itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
                {
                    _inventory.UseItem(item);
                };
                // Drop item
                Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount };
                itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
                {
                    _inventory.RemoveItem(item);
                    CollectableManager.DropItem(_player.GetPosition(), duplicateItem);
                };                
                
                itemSlotRectTransform.anchoredPosition =
                    new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize); // Negate y for proper UI layout
                var image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
                if (image == null)
                    Debug.LogError("UIInventory: Image component not found on ItemSlotTemplate!");
                else if (item.GetSprite() == null)
                    Debug.LogError($"UIInventory: Sprite for item {item} is null!");
                else
                    image.sprite = item.GetSprite();
                
                TextMeshProUGUI uiText = itemSlotRectTransform.Find("Amount").GetComponent<TextMeshProUGUI>();
                uiText.SetText(item.amount > 1 ? item.amount.ToString() : "");

                x++; // Move to the next column
                if (x < maxColumns) continue; // Move to the next row when max columns are reached
                x = 0;
                y++;
            }
        }

        private void ValidateUIElements()
        {
            if (itemSlotContainer == null)
                Debug.LogError("UIInventory: ItemSlotContainers not found! Check the hierarchy.");
            if (itemSlotTemplate == null)
                Debug.LogError("UIInventory: ItemSlotTemplates not found! Check the hierarchy.");
        }

        private bool ValidateInventory()
        {
            if (_inventory == null)
                Debug.LogError("UIInventory: _inventory is null! SetInventory() might not have been called.");
            if (itemSlotContainer == null)
                Debug.LogError("UIInventory: itemSlotContainer is null. Check UI hierarchy.");
            if (itemSlotTemplate == null) Debug.LogError("UIInventory: itemSlotTemplate is null. Check UI hierarchy.");
            if (_inventory == null) return true;
            var itemList = _inventory.GetItemList();
            if (_inventory == null) return true;
            if (itemList == null) Debug.LogError("UIInventory: GetItemList() returned null!");
            if (itemList != null) Debug.Log($"UIInventory: Refreshing inventory. Item count: {itemList.Count}");
            return true;
        }
    }
}