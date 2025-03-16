using System;
using UnityEngine;

namespace _Project._Scripts.Units.Puzzles
{
    public class Doors : MonoBehaviour
    {
        private static readonly int OpenDoor = Animator.StringToHash("OpenDoor");
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>(); // Get the Animator component
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                animator.SetTrigger(OpenDoor); // Set the trigger to play animation
            }
        }
    }
}