using UnityEngine;
using System.Collections;
namespace ScriptUtils.Interface
{
    /// <summary>
    /// Helper MonoBehaivour used for reseting Navigor if its needed, also used to start coruitines in non MonoBehuivour classes used for navigation.
    /// A gameObject with this componennt attached will be created by Navigator as its needed.
    /// </summary>
    public class NavigationEventManager : MonoBehaviour
    {
        /// <summary>
        /// Current instance of helper class, set from Awake
        /// </summary>
        public static NavigationEventManager Instance;
        private void Awake()
        {
            Instance = this;
        }
        /// <summary>
        /// Resets the Navigor in case this component or parent gameobject is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            NavigationEventSystem.reset();
        }
    }
}