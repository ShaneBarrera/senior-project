using UnityEngine;
using _Project._Scripts.Managers.Systems; // Required for IResettable

/****************************************************
 *               BOOL VALUE CLASS                  *
 ****************************************************
 * Description: This class holds a boolean value   *
 * that can be initialized and used at runtime,    *
 * with support for Unity's serialization system.  *
 * It is used to store a boolean value that can    *
 * be reset to its initial value after deserialization. *
 *                                                  *
 * Features:                                        *
 * - Stores an initial boolean value                *
 * - Holds a runtime boolean value                  *
 * - Resets the runtime value to initial value after*
 *   deserialization                                 *
 ****************************************************/

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BoolValue", menuName = "ScriptableObjects/BoolValue", order = 1)]
    public class BoolValue : ScriptableObject, ISerializationCallbackReceiver, IResettable
    {
        /******************************************
         *           INITIAL AND RUNTIME VALUE    *
         ******************************************/
        public bool initialValue;

        [HideInInspector]
        public bool runtimeValue;

        /******************************************
         *     SERIALIZATION CALLBACK METHODS    *
         ******************************************/
        public void OnAfterDeserialize()
        {
            runtimeValue = initialValue;
        }

        public void OnBeforeSerialize() {}

        /******************************************
         *        RESETTABLE IMPLEMENTATION       *
         ******************************************/
        public void ResetToDefault()
        {
            runtimeValue = initialValue;
        }
    }
}