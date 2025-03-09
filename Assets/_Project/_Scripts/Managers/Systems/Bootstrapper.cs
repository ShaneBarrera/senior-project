using UnityEngine;

/****************************************************
 *                  BOOTSTRAPPER CLASS              *
 ****************************************************
 * Description: This class ensures that a specified *
 * system object is instantiated and persists across *
 * scene loads in the game. It uses the Unity runtime*
 * initialization method to instantiate the object  *
 * from resources and prevent it from being destroyed*
 * when new scenes are loaded.                     *
 *                                                 *
 * Features:                                       *
 * - Instantiates a system object before the scene *
 *   is loaded                                    *
 * - Ensures the system object is not destroyed    *
 *   when loading new scenes                      *
 * - Uses the [RuntimeInitializeOnLoadMethod]       *
 *   attribute to execute at runtime              *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public static class Bootstrapper {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute() {
            Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems")));
        }
    }
}