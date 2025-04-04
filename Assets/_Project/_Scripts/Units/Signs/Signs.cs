using _Project._Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;

/****************************************************
 *                 SIGN INTERACTION                 *
 ****************************************************
 * Description: This class manages interactions    *
 * with in-game signs. When a player enters the    *
 * sign's trigger area, a dialogue box appears,    *
 * displaying a predefined message. The text box   *
 * disappears when the player leaves the area.     *
 *                                                 *
 * Features:                                       *
 * - Displays dialogue when player enters range    *
 * - Hides text box when player exits range        *
 * - Prevents flickering or repeated activations   *
 * - Uses serialized fields for easy customization *
 ****************************************************/

namespace _Project._Scripts.Units.Signs
{
    public class Signs : MonoBehaviour
    {
        // ProText GUI
        [SerializeField] private GameObject textBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private string dialogue;
        
        public SignalSender context;
        private void Start() 
        {
            textBox.SetActive(false); // Ensure text box is off at the start
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            context.Raise();
            textBox.SetActive(true);
            dialogueText.text = dialogue;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            context.Raise();
            textBox.SetActive(false);
        }
    }
}