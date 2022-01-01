namespace EduUtils.Events
{
    /// <summary>
    /// Used to determine the type of mouse event being dispatched.
    /// </summary>
    public enum MouseEventType
    {
        /// <summary>
        /// Dispatched when users clicks a target, synonymouse with UP for now
        /// </summary>
        CLICK,
        /// <summary>
        /// Dispatched when user click twice in quick succession
        /// </summary>
        DOUBLE_CLICK,
        /// <summary>
        /// Dispatched when a user is hovering over a target(after at least one frame)
        /// </summary>
        OVER,
        /// <summary>
        /// Dispathed when Mouse curser enters a new target.
        /// </summary>
        ENTER,
        /// <summary>
        /// Dispatched when users start pressing a mouse button while over a target.
        /// </summary>
        DOWN,
        /// <summary>
        /// Dispatched once per frame while user is holding down a mouse button over a target. (after at least one frame)
        /// </summary>
        PRESSED,
        /// <summary>
        /// Dispatched when users release a mouse button while over a target
        /// </summary>
        UP,
        /// <summary>
        /// Dispathed when Mouse curser exits a target.
        /// </summary>
        EXIT
    }
}