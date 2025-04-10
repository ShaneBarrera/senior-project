using System;
using System.Collections;
using _Project._Scripts.Units.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

/****************************************************
 *                 DEATH MANAGER                   *
 ****************************************************
 * Description: Manages the player's death state.  *
 * When the player is killed by an enemy, this     *
 * class activates a UI panel and handles the      *
 * transition back to the main menu.               *
 *                                                 *
 * Features:                                       *
 * - Shows death panel on death                    *
 * - Disables movement & freezes time              *
 * - Resets ScriptableObjects on returning to menu *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class DeathManager : MonoBehaviour
    {
        [Header("UI Components")]
        public GameObject deathPanel;

        [Header("Scene Management")]
        public string mainMenuScene;

        private Player _player;

        [Obsolete("Start is obsolete")]
        private void Start()
        {
            if (deathPanel != null)
                deathPanel.SetActive(false);

            _player = FindObjectOfType<Player>();
        }

        public void TriggerDeath()
        {
            if (_player != null)
                _player.enabled = false;

            Cursor.visible = true;
            StartCoroutine(ShowDeathScreenAfterDelay());
        }

        private IEnumerator ShowDeathScreenAfterDelay()
        {
            deathPanel?.SetActive(true);

            // Let UI render for a frame before freezing time
            yield return new WaitForEndOfFrame();

            Time.timeScale = 0f;
        }

        public void ReturnToMainMenu()
        {
            // Reset all ScriptableObject data
            GameSaveManager.GameSave?.ResetAllData();

            Cursor.visible = true;
            Time.timeScale = 1f;

            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
