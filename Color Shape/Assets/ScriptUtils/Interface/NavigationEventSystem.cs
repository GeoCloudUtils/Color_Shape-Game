using UnityEngine;
using System.Collections;
/// <summary>
/// Contains classes for events used by Navigoator and related systems.
/// </summary>
namespace ScriptUtils.Interface
{
    /// <summary>
    /// Main logic for Navigation events (as in dispatching them).
    /// Some non related classes may use this to determine when a navigation action completes.
    /// Note: Implements the Singelton design pattern.
    /// </summary>
    public class NavigationEventSystem
    {
        /// <summary>
        /// used to store current instance for the Singelton design pattern
        /// </summary>
        private static NavigationEventSystem instance;
        /// <summary>
        /// Reference to the event manager helper class. Created automaicly.
        /// </summary>
        private NavigationEventManager manager;
        /// <summary>
        /// Definition for the delegate used for event
        /// </summary>
        /// <param name="evParam"></param>
        public delegate void NavigationDelegate(NavigationEventParams evParam);
        /// <summary>
        /// Dispatched when navigation action starts
        /// </summary>
        public event NavigationDelegate NavigationStartEvent;
        /// <summary>
        /// Dispatched when navigation action completes
        /// </summary>
        public event NavigationDelegate NavigationCompleteEvent;

        /// <summary>
        /// Sets singleton instance
        /// </summary>
        private NavigationEventSystem()
        {
            instance = this;
        }
        /// <summary>
        /// Gets or creates and gets instance. Also handels creation of the helper class if needed.
        /// </summary>
        /// <returns></returns>

        public static NavigationEventSystem getInstance()
        {
            if (instance == null)
                new NavigationEventSystem();
            instance.checkOrCreateManager();
            return instance;
        }
        /// <summary>
        /// Creates a new instance.
        /// Note All event listenres are remove before satting instance to new instance.
        /// </summary>
        public static void reset()
        {
            instance.NavigationStartEvent = null;
            instance.NavigationCompleteEvent = null;
            instance.manager = null;
            new NavigationEventSystem();
        }
        /// <summary>
        /// Checks if the manager exists, if it doesnt, it will create a new gameObject called NAVIGATION_EVENT_MANAGER and adds the helper class component to it.
        /// If an instance already exist this function will do nothing.
        /// </summary>
        public void checkOrCreateManager()
        {
            if (manager == null)
            {
                GameObject managerGameObject = new GameObject("NAVIGATION_EVENT_MANAGER");
                manager = managerGameObject.AddComponent<NavigationEventManager>();
            }
        }
        /// <summary>
        /// Dispatches a navigation start event.
        /// Note this function should only be called by Navigaotr and related systems not manualy.
        /// </summary>
        /// <param name="param"></param>
        public void dispatchNavivigationStartEvent(NavigationEventParams param)
        {
            Navigator.getInstance();
            if (NavigationStartEvent != null)
                NavigationStartEvent(param);
        }
        /// <summary>
        /// Dispatches a navigation complete event.
        /// Note this function should only be called by Navigaotr and related systems not manualy.
        /// </summary>
        /// <param name="param"></param>
        public void dispatchNavivigationCompleteEvent(NavigationEventParams param)
        {
            if (NavigationCompleteEvent != null)
                NavigationCompleteEvent(param);
        }
    }
}