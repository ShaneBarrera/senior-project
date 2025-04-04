using UnityEngine;
using UnityEngine.SceneManagement;

/****************************************************
 *                  MAIN MENU                      *
 ****************************************************
 * Description: This class manages the main menu  *
 * functionality, allowing players to start a new *
 * game or exit the application. It handles scene *
 * transitions and application quitting.          *
 *                                                 *
 * Features:                                       *
 * - Loads the game scene when "New Game" is      *
 *   selected                                     *
 * - Exits the application when "Exit" is chosen  *
 * - Simple and efficient menu management         *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class MainMenu : MonoBehaviour
    {
        public void NewGame()
        {
            SceneManager.LoadScene("MansionLobby");
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
