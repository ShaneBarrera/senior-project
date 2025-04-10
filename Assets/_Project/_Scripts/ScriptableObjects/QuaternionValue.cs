using UnityEngine;
using _Project._Scripts.Managers.Systems; // for IResettable

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "QuaternionValue", menuName = "ScriptableObjects/Quaternion", order = 1)]
    public class QuaternionValue : ScriptableObject, IResettable, ISerializationCallbackReceiver
    {
        [Header("Quaternion Values")]
        public Quaternion initialValue;
        public Quaternion defaultValue;

        /******************************************
         *       ISerializationCallbackReceiver
         ******************************************/
        public void OnAfterDeserialize()
        {
            initialValue = defaultValue;
        }

        public void OnBeforeSerialize() { }

        /******************************************
         *              IResettable
         ******************************************/
        public void ResetToDefault()
        {
            initialValue = defaultValue;
        }
    }
}