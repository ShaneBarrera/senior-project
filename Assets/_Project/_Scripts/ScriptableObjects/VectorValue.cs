using UnityEngine;

/****************************************************
 *                VECTOR VALUE SCRIPTABLE OBJECT    *
 ****************************************************
 * Description: This ScriptableObject stores and   *
 * manages a Vector2 value that can persist across *
 * different scenes or game states. It is useful   *
 * for saving player positions, spawn points, or   *
 * other positional data that needs to reset when  *
 * the game reloads.                               *
 *                                                 *
 * Features:                                       *
 * - Stores an initial Vector2 value               *
 * - Maintains a default value for reset states    *
 * - Resets initialValue to defaultValue on load   *
 * - Implements ISerializationCallbackReceiver for *
 *   automatic value resetting                     *
 ****************************************************/

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "VectorValue", menuName = "ScriptableObjects/VectorValue", order = 1)]
    public class VectorValue : ScriptableObject, ISerializationCallbackReceiver
    {
        public Vector2 initialValue;
        public Vector2 defaultValue;
        
        public void OnAfterDeserialize()
        {
         initialValue = defaultValue;;   
        }

        public void OnBeforeSerialize() {}

    }
}