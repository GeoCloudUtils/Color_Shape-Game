using UnityEngine;
using System.Collections;
namespace ScriptUtils.Interface
{
    public class NavigationEventParams
    {
        /// <summary>
        /// Used to denote if this event was triggered by the start of a navigation action or its completion.
        /// </summary>
        public enum EventType
        {
            START,
            COMPLETE
        }
        /// <summary>
        /// Used to denote the type of the target scene
        /// </summary>
        public enum NavigationTargetType
        {
            CAPITOL,
            GAME,
            GAME_DIFICULTY,
            MOVIE,
            MAIN
        }
        /// <summary>
        /// Type of the Event
        /// </summary>
        public EventType type;
        /// <summary>
        /// Name of the target scene
        /// </summary>
        public string navigationTarget;
        /// <summary>
        /// Type of the target scene
        /// </summary>
        public NavigationTargetType targetType;
        /// <summary>
        /// Shortcut to set fields.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="targetType"></param>
        /// <param name="navigationTarget"></param>
        public NavigationEventParams(EventType type, NavigationTargetType targetType, string navigationTarget = "")
        {
            this.type = type;
            this.navigationTarget = navigationTarget;
            this.targetType = targetType;
        }
    }
}