using System.Collections;
using UnityEngine;

namespace ScriptUtils.Visual
{
    /// <summary>
    /// Destroys GameObject when attached particle system is done (detected using ParticleSystemEvent)
    /// Destruction will be delayed by 1 frame.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemCleaner : ParticleCleanerEvent
    {
        private void Awake()
        {
            ParticleSystemDone += ParticleSystemCleaner_ParticleSystemDone;
        }

        void ParticleSystemCleaner_ParticleSystemDone(ParticleCleanerEvent target)
        {
            StartCoroutine(destroyMe());
        }
        public override void ResetEvent()
        {
            base.ResetEvent();
            this.ParticleSystemDone += ParticleSystemCleaner_ParticleSystemDone;
        }
        private IEnumerator destroyMe()
        {
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);
        }
    }
}