using UnityEngine;

/****************************************************
 *              COLLECTABLE SPAWNER                 *
 ****************************************************
 * Description: This class is responsible for      *
 * spawning collectable items in the game world.   *
 * It ensures that an item is assigned before      *
 * spawning and delegates the spawning process     *
 * to the CollectableManager.                      *
 *                                                 *
 * Features:                                       *
 * - Spawns collectable items at designated spots  *
 * - Ensures item assignment before spawning       *
 * - Uses CollectableManager for item management   *
 * - Destroys itself after spawning the item       *
 ***************************************************
 * Note: sprites in the Collectable Spawner are    *
 * only placeholders; the actual rendered sprites  *
 * are determined by ItemAssets.cs in Unity        *
 * The actual size of sprites rendered is determined
 * by pfCollectables prefab in Unity collectables  *
 * folder!                                         *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class CollectableSpawner : MonoBehaviour
    {
        public Item item;

        private void Awake()
        {
            if (item == null)
            {
                Debug.LogError("Item is null in ItemWorldSpawner.");
                return;
            }

            CollectableManager.SpawnCollectableManager(transform.position, item);
            Destroy(gameObject);
        }
    }
}