using UnityEngine;

/****************************************************
 *                ITEM ASSETS MANAGER               *
 ****************************************************
 * Description: This singleton class provides a    *
 * centralized reference for item-related assets   *
 * used in the game. It ensures easy access to     *
 * item sprites and collectable prefabs, enabling  *
 * consistent item representation across the game. *
 *                                                 *
 * Features:                                       *
 * - Singleton pattern for global asset access     *
 * - Stores references to essential item sprites   *
 * - Provides prefab reference for collectables    *
 * - Ensures only one instance exists              *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class ItemAssets : MonoBehaviour
    {
        public static ItemAssets Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public Transform pfCollectables;
        public Sprite keySprite;
        public Sprite healthSprite;
        public Sprite medKitSprite;
        public Sprite ammoSprite;
    }
}