using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace EduUtils.Events
{
    /// <summary>
    /// Used for dispatching events at the end(or other predefined moments) of/in animations/tweens.
    /// </summary>
    public class ArbitraryEvent : MonoBehaviour
    {
        public event UnityAction<GameObject> Done;

        /// <summary>
        /// Should be called by DOTween or Animator (configured in Editor)
        /// </summary>
        public void dispatchEvent()
        {
            if (Done != null)
                Done(gameObject);
        }
    }
}