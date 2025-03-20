using UnityEngine;

namespace _Project._Scripts.ScriptableObjects
{
    public class ContextClue : MonoBehaviour
    {
        public GameObject contextClue;
        public bool contextClueActive = false;

        public void ChangeContext()
        {
            contextClueActive = !contextClueActive;
            contextClue.SetActive(contextClueActive);
        }
    }
}
