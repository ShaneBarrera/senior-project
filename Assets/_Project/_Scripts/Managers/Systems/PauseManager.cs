using System;
using _Project._Scripts.Units.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


/****************************************************
 *                  PAUSE MANAGER                  *
 ****************************************************
 * Description: This class handles pausing and    *
 * resuming the game. It displays a pause menu,   *
 * manages time scaling, and controls player      *
 * movement during pauses. Additionally, it       *
 * provides an option to return to the main menu. *
 *                                                 *
 * Features:                                       *
 * - Toggles pause state with the Escape key      *
 * - Disables player movement when paused         *
 * - Freezes and resumes game time accordingly    *
 * - Shows and hides the pause menu               *
 * - Allows returning to the main menu            *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class PauseManager : MonoBehaviour
    {
        /******************************************
         *         EXTERNAL REFERENCES            *
         ******************************************/
        public GameObject pausePanel;
        private Player _playerMovement;
        
        /******************************************
         *          SCENE AND STATE MANAGEMENT    *
         ******************************************/
        public string mainMenuScene;
        private bool _isPaused;

        /******************************************
         *             START FUNCTION             *
         ******************************************/
        [Obsolete("Obsolete")]
        public void Start()
        {
            _isPaused = false;
            pausePanel.SetActive(false);
            Cursor.visible = false;
            
            // Find the player script
            _playerMovement = FindObjectOfType<Player>();
        }

        /******************************************
         *               UPDATE FUNCTION          *
         ******************************************/
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ChangePause();
            }
        }

        /******************************************
         *           PAUSE TOGGLE FUNCTION        *
         ******************************************/
        public void ChangePause()
        {
            _isPaused = !_isPaused;

            foreach (var ambient in AmbientSound3D.ActiveInstances)
            {
                if (ambient is null) continue;
                ambient.FadeToVolume(_isPaused ? 0f : 1f, 0.75f);
            }

            if (_isPaused)
            {
                pausePanel.SetActive(true);
                Cursor.visible = true;
                Time.timeScale = 0f;

                // Ensure no button remains "selected" so hover works again
                EventSystem.current.SetSelectedGameObject(null);

                if (_playerMovement)
                    _playerMovement.enabled = false;
            }
            else
            {
                pausePanel.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1f;

                // Clear selected UI to prevent ghost highlighting
                EventSystem.current.SetSelectedGameObject(null);

                if (_playerMovement)
                    _playerMovement.enabled = true;
            }
        }

        /******************************************
         *           EXIT GAME FUNCTION           *
         ******************************************/
        public void ExitGame()
        {
            SceneManager.LoadScene(mainMenuScene);
            Cursor.visible = true;
            Time.timeScale = 1f;
        }
    }
}
