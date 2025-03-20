using System.Collections.Generic;
using _Project._Scripts.Managers.Systems;
using UnityEngine;

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Signal", menuName = "ScriptableObjects/Signal")]
    public class SignalSender : ScriptableObject
    {
        public List<SignalListener> listeners = new List<SignalListener>();

        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnSignalRaised();
            }
        }

        public void RegisterListener(SignalListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(SignalListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
