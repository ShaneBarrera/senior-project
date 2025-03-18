using UnityEngine;

namespace _Project._Scripts.Units.Enemy
{
    public class Enemy : MonoBehaviour
    {
        
        public int baseAttack;
        public float moveSpeed;
        public float chaseRadius;
        public string enemyName;
        
        public Transform target;
        public Transform homePosition;

        public void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public void Update()
        {
            CheckDistance();
        }

        private void CheckDistance()
        {
            if (Vector3.Distance(transform.position, target.position) <= chaseRadius)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }
        }
    }
}
