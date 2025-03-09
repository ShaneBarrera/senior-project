using UnityEngine;
using System;

/****************************************************
 *                  ITEM CLASS                      *
 ****************************************************
 * Description: This class represents an item in   *
 * the game. Each item has a type and an amount,   *
 * and it provides a method to retrieve the        *
 * corresponding sprite from the ItemAssets class. *
 *                                                 *
 * Features:                                       *
 * - Defines multiple item types                   *
 * - Stores item type and quantity                 *
 * - Retrieves item sprite dynamically             *
 * - Integrates with the ItemAssets singleton      *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    [Serializable]
    public class Item
    {
        public enum ItemType
        {
            Key,
            Health,
            Medkit,
            Ammo
        }

        public ItemType itemType;
        public int amount;

        public Sprite GetSprite()
        {
            switch (itemType)
            {
                default:
                case ItemType.Key:
                    return ItemAssets.Instance.keySprite;
                case ItemType.Health:
                    return ItemAssets.Instance.healthSprite;
                case ItemType.Medkit:
                    return ItemAssets.Instance.medKitSprite;
                case ItemType.Ammo:
                    return ItemAssets.Instance.ammoSprite;
            }
        }

        public bool IsStackable()
        {
            switch (itemType)
            {
                default:
                case ItemType.Health:
                case ItemType.Medkit:
                case ItemType.Ammo:
                    return true;
                case ItemType.Key:
                    return false;
            }
        }
    }
}