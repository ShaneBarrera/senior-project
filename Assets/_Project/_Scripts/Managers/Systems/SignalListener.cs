using _Project._Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace _Project._Scripts.Managers.Systems
{
    public class SignalListener : MonoBehaviour
    {
        public SignalSender signal;
        public UnityEvent signalEvent;
        public void OnSignalRaised()
        {
            signalEvent.Invoke();
        }

        private void OnEnable()
        {
            signal.RegisterListener(this);
        }

        private void OnDisable()
        {
            signal.UnregisterListener(this);
        }
    }
}
