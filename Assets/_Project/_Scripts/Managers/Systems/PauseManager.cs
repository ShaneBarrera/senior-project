using System;
using _Project._Scripts.Units.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (_isPaused)
            {
                pausePanel.SetActive(true);
                Cursor.visible = true;
                Time.timeScale = 0f;
                if (_playerMovement)
                    _playerMovement.enabled = false; // Disable movement
            }
            else
            {
                pausePanel.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1f;
                if (_playerMovement)
                    _playerMovement.enabled = true; // Re-enable movement
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
