using UnityEngine;

namespace _Project._Scripts.ScriptableObjects
{
    /****************************************************
     *           ENEMY BEHAVIOR PROFILE SO            *
     ****************************************************
     * Description: This scriptable object defines     *
     * enemy attributes for easy scene-specific tuning *
     * or per-enemy customization.                     *
     *                                                  *
     * Features:                                        *
     * - Stores movement speed, chase range, roam range*
     * - Optional behavior modifiers for variety       *
     ****************************************************/

    [CreateAssetMenu(fileName = "EnemyBehaviorProfile", menuName = "ScriptableObjects/EnemyBehaviorProfile", order = 1)]
    public class EnemyBehaviorProfile : ScriptableObject
    {
        [Header("Movement Settings")]
        public float moveSpeed = 2.5f;
        public float chaseRadius = 5f;
        public float idleBufferRadius = 0.5f;
        public float roamRadius = 2f;

        [Header("Behavioral Variants")]
        public float stalkRadius = 3f; // Optional stalking behavior
        public bool isAggressive = true;

        [Header("Randomization")]
        public bool allowAttributeRandomization = false;
        public float moveSpeedVariance = 0.5f;
        public float chaseRadiusVariance = 1f;
    }
}