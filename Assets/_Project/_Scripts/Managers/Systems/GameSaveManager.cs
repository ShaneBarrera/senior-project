using System.Collections.Generic;
using UnityEngine;

namespace _Project._Scripts.Managers.Systems
{
    public class GameSaveManager : MonoBehaviour
    {
        public static GameSaveManager GameSave;

        public List<ScriptableObject> objects = new List<ScriptableObject>();
        void Awake()
        {
            if (GameSave == null)
            {
                GameSave = this;
            }
            else
            {
                Destroy(this);
            }
           
        }
    }
}
