using _Project._Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

/****************************************************
 *               SIGNAL LISTENER CLASS             *
 ****************************************************
 * Description: This class listens for signals     *
 * raised by a SignalSender and triggers assigned  *
 * UnityEvents in response. It registers itself    *
 * as a listener when enabled and unregisters      *
 * when disabled.                                  *
 *                                                 *
 * Features:                                       *
 * - Listens for signals from a SignalSender       *
 * - Invokes UnityEvents when a signal is raised   *
 * - Registers and unregisters itself dynamically  *
 ****************************************************/

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
