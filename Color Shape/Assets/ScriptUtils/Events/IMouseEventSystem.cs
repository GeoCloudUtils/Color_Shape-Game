using UnityEngine;

namespace EduUtils.Events
{
    /// <summary>
    /// Base interface for any mouse event class that will be used in MouseEventProcessor.
    /// </summary>
    public interface IMouseEventSystem
    {
        bool entered
        {
            get;
            set;
        }
		bool canProces
		{
			get;
		}
        void dispatchMouse(MouseEventType type);
    }
}