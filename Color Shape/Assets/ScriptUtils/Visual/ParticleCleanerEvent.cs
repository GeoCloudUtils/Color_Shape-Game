using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils.Visual
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleCleanerEvent : MonoBehaviour
    {
        /// <summary>
        /// Used for event.
        /// </summary>
        /// <param name="target">The instance of this class that dispatched the event.</param>
        public delegate void ParticlySystemDoneDelegate(ParticleCleanerEvent target);
        /// <summary>
        /// Dispatched when the paritcle system is done.
        /// </summary>
        public event ParticlySystemDoneDelegate ParticleSystemDone;
        /// <summary>
        /// The maximum amount of time particle system should run before event is dispatched.
        /// Default is -1.
        /// If value is -1 no such restriction is applied.
        /// </summary>
        public float maxLifeTime = -1f;
        /// <summary>
        /// Gets the attached particle system.
        /// </summary>
        public ParticleSystem attachedParticleSystem
        {
            get
            {
                if (pSystem == null)
                    pSystem = gameObject.GetComponent<ParticleSystem>();
                return pSystem;
            }
        }

        /// <summary>
        /// Time based on Time.time when the particle system started playing.
        /// </summary>
        private float startTime = 0f;
        /// <summary>
        /// Makes sure event is not fired continously after the particle system is compelte.
        /// </summary>
        private bool dispatchedDone = false;
        private ParticleSystem pSystem;

        private void Awake()
        {
            if (!attachedParticleSystem.main.playOnAwake)
                dispatchedDone = true;
        }
        /// <summary>
        /// Resets the ParitcleSystemEvent instance removing all attached listenres and  setiing dispatchedDone to false.
        /// </summary>
        public virtual void ResetEvent()
        {
            ParticleSystemDone = null;
            dispatchedDone = false;
        }
        /// <summary>
        /// Dispatches the event based on maxLifeTime (if it is !=-1) or when the particle system IsAlive.
        /// </summary>
        void Update()
        {
            if (pSystem == null)
                pSystem = gameObject.GetComponent<ParticleSystem>();
            if (pSystem.IsAlive())
                dispatchedDone = false;
            if (maxLifeTime != -1)
            {
                if (startTime == 0f && !pSystem.isStopped)
                    startTime = Time.time;
                if (Time.time - startTime >= maxLifeTime)
                {
                    if (!dispatchedDone)
                    {
                        if (ParticleSystemDone != null)
                            ParticleSystemDone(this);
                        dispatchedDone = true;
                    }
                    startTime = 0f;
                    pSystem.Stop();
                    return;
                }
            }
            if (!pSystem.IsAlive() && !pSystem.isPaused)
            {
                if (!dispatchedDone)
                {
                    if (ParticleSystemDone != null)
                        ParticleSystemDone(this);
                    pSystem.Stop();
                    dispatchedDone = true;
                }
            }
        }
    }
}