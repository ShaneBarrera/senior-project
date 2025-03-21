using System.Collections.Generic;
using _Project._Scripts.Managers.Systems;
using UnityEngine;

/****************************************************
 *                SIGNAL SENDER CLASS              *
 ****************************************************
 * Description: This class acts as an event system *
 * that notifies registered listeners when a       *
 * signal is raised. It enables decoupled event    *
 * handling across different game objects.         *
 *                                                 *
 * Features:                                       *
 * - Raises signals to notify all listeners        *
 * - Maintains a list of registered listeners      *
 * - Supports adding and removing listeners        *
 ****************************************************/

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Signal", menuName = "ScriptableObjects/Signal")]
    public class SignalSender : ScriptableObject
    {
        public List<SignalListener> listeners = new List<SignalListener>();

        public void Raise()
        {
            for (var i = listeners.Count - 1; i >= 0; i--)
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
