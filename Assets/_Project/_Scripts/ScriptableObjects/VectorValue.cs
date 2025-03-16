using UnityEngine;

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