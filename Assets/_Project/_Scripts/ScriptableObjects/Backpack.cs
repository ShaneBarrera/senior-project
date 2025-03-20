using System.Collections.Generic;
using UnityEngine;

// Equivalent to an Inventory.cs class 

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Backpack", menuName = "ScriptableObjects/Backpack", order = 1)]
    public class Backpack : ScriptableObject
    {
        public Thing currentThing;
        public List<Thing> things = new List<Thing>();
        public int numberOfKeys;

        public void AddThing(Thing thing)
        {
            if (thing.isKey)
            {
                numberOfKeys++;
            }
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
