using UnityEngine;

/****************************************************
 *                CONTEXT CLUE CLASS               *
 ****************************************************
 * Description: This class manages the visibility  *
 * of context clues in the game, toggling their    *
 * active state based on player interactions.      *
 *                                                 *
 * Features:                                       *
 * - Toggles context clue visibility               *
 * - Uses a boolean flag to track active state     *
 * - Activates or deactivates a GameObject         *
 ****************************************************/

namespace _Project._Scripts.ScriptableObjects
{
    public class ContextClue : MonoBehaviour
    {
        public GameObject contextClue;
        public bool contextClueActive = false;

        public void ChangeContext()
        {
            contextClueActive = !contextClueActive;
            contextClue.SetActive(contextClueActive);
        }
    }
}
