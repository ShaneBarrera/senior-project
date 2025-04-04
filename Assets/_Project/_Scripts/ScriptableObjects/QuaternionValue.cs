using UnityEngine;

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "QuaternionValue", menuName = "ScriptableObjects/Quaternion", order = 1)]
    public class QuaternionValue : ScriptableObject
    {
        public Quaternion initialValue;
    }
}