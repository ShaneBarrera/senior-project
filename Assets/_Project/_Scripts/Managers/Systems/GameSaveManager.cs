using System.Collections.Generic;
using UnityEngine;

/****************************************************
 *               GAME SAVE MANAGER                 *
 ****************************************************
 * Description: This class manages game-saving     *
 * functionalities, including resetting Scriptable *
 * Objects when starting a new game or after death.*
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class GameSaveManager : MonoBehaviour
    {
        public static GameSaveManager GameSave;

        [Header("Player Attributes")]
        public List<ScriptableObject> playerAttributes = new List<ScriptableObject>();

        [Header("Keys & Inventory")]
        public List<ScriptableObject> inventoryData = new List<ScriptableObject>();

        [Header("Interactable State (Chests, Switches, etc.)")]
        public List<ScriptableObject> interactableStates = new List<ScriptableObject>();

        [Header("World State / Scene Info")]
        public List<ScriptableObject> worldState = new List<ScriptableObject>();

        private void Awake()
        {
            if (GameSave == null)
            {
                GameSave = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /******************************************
         *         RESET ALL DATA TO DEFAULT      *
         ******************************************/
        public void ResetAllData()
        {
            ResetList(playerAttributes);
            ResetList(inventoryData);
            ResetList(interactableStates);
            ResetList(worldState);
        }

        private static void ResetList(List<ScriptableObject> list)
        {
            foreach (var obj in list)
            {
                if (obj is IResettable resettable)
                {
                    resettable.ResetToDefault();
                }
            }
        }
    }
}
