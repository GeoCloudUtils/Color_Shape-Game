using UnityEngine;
using System.Collections;
namespace ScriptUtils.Events
{
    /// <summary>
    /// Used for collision detection.
    /// Event names are self explanatory
    /// </summary>
    public class ColliderEventSystem : MonoBehaviour
    {
        /// <summary>
        /// Used by all events in ColliderEventSystem
        /// </summary>
        /// <param name="eventTarget">ColliderEventSystem that dispathed the event</param>
        /// <param name="other">Collider2D that hit the eventTarget</param>
        public delegate void ColliderDelegate (ColliderEventSystem eventTarget, Collider2D other);

        public event ColliderDelegate TriggerEntered;
        public event ColliderDelegate TriggerExited;
        public event ColliderDelegate ColliderEntered;
        public event ColliderDelegate ColliderExited;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (TriggerEntered != null)
                TriggerEntered(this, collider);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (TriggerExited != null)
                TriggerExited(this, collider);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (ColliderEntered != null)
                ColliderEntered(this, col.collider);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (ColliderExited != null)
                ColliderExited(this, collision.collider);
        }
    }
}