using _Project._Scripts.Utilities;
using UnityEngine;

/****************************************************
 *              COLLECTABLE MANAGER CLASS           *
 ****************************************************
 * Description: This class handles the spawning,    *
 * management, and destruction of collectable      *
 * items in the game. It assigns the correct sprite*
 * to the collectable item and provides methods to *
 * retrieve or remove the item when collected.     *
 *                                                 *
 * Features:                                       *
 * - Spawns collectable items dynamically          *
 * - Assigns correct item sprite                   *
 * - Retrieves item details                        *
 * - Supports item destruction upon collection     *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class CollectableManager : MonoBehaviour
    {
        private Item _item;
        private SpriteRenderer _spriteRenderer;

        public static CollectableManager SpawnCollectableManager(Vector3 position, Item item)
        {
            Transform transform = Instantiate(ItemAssets.Instance.pfCollectables, position, Quaternion.identity);
            CollectableManager collectableManager = transform.GetComponent<CollectableManager>();
            collectableManager.SetItem(item);
            return collectableManager;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public static void DropItem(Vector3 dropPosition, Item item)
        {
            Vector3 randomDirection = UtilsClass.GetRandomDir();
            CollectableManager collectableManager = SpawnCollectableManager(dropPosition + randomDirection * 2.0f, item);
            collectableManager.GetComponent<Rigidbody2D>().AddForce(randomDirection * 2.0f, ForceMode2D.Impulse);
        }
        
        private void SetItem(Item item)
        {
            _item = item;
            _spriteRenderer.sprite = _item.GetSprite();
        }

        public Item GetItem()
        {
            return _item;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}