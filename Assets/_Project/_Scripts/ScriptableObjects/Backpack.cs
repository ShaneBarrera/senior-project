using System.Collections.Generic; 
using UnityEngine;
using _Project._Scripts.Managers.Systems;

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
 * - Resets state on player death via IResettable  *
 ****************************************************/

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Backpack", menuName = "ScriptableObjects/Backpack", order = 1)]
    public class Backpack : ScriptableObject, IResettable
    {
        /******************************************
         *             COLLECTED ITEMS            *
         ******************************************/
        public Thing currentThing;
        public List<Thing> things = new List<Thing>();

        /******************************************
         *               KEY TRACKING             *
         ******************************************/
        public bool hasStandardKey;
        public bool hasSilverKey;
        public bool hasBronzeKey;
        public bool hasBloodyKey;

        /******************************************
         *            ADD ITEM TO BACKPACK        *
         ******************************************/
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

        /******************************************
         *             RESET TO DEFAULT           *
         ******************************************/
        public void ResetToDefault()
        {
            currentThing = null;
            things.Clear();

            hasStandardKey = false;
            hasSilverKey = false;
            hasBronzeKey = false;
            hasBloodyKey = false;
        }
    }
}