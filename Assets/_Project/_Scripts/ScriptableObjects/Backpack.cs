using System.Collections.Generic;
using UnityEngine;

/****************************************************
 *                  BACKPACK CLASS                 *
 ****************************************************
 * Description: This class serves as an inventory  *
 * system for the player, storing collected items  *
 * including keys. It keeps track of the number of *
 * keys and manages a list of collected items.     *
 *                                                 *
 * Features:                                       *
 * - Stores collected items in a list              *
 * - Tracks the number of keys separately          *
 * - Prevents duplicate non-key items in inventory *
 * - Allows adding new items dynamically           *
 ****************************************************/


namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Backpack", menuName = "ScriptableObjects/Backpack", order = 1)]
    public class Backpack : ScriptableObject
    {
        // Items collectable in backpack
        public Thing currentThing;
        public List<Thing> things = new List<Thing>();
        
        // Validate presence of current item
        public bool hasStandardKey;
        public bool hasSilverKey;
        public bool hasBronzeKey;
        public bool hasBloodyKey;

        public void AddThing(Thing thing)
        {
            // Key cases
            if (thing.isStandardKey)
            {
                hasStandardKey = true;
            }
            if (thing.isSilverKey)
            {
                hasSilverKey = true;
            }
            if (thing.isBronzeKey)
            {
                hasBronzeKey = true;
            }
            if (thing.isBloodyKey)
            {
                hasBloodyKey = true;
            }
            // <insert new cases here>
            else
            {
                if (!things.Contains(thing))
                {
                    things.Add(thing);
                }
            }
        }
    }
}
