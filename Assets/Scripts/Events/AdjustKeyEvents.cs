using System;

namespace Assets.Scripts.Events
{
    /// <summary>
    /// TODO: add a link here
    /// </summary>
    public static class AdjustKeyEvents
    {
        public static event Action<bool> OnRotation;
        public static event Action OnReset;

        /// <summary>
        /// Invokes the action associated with board rotation.
        /// </summary>
        /// <param name="movedRight">if set to <c>true</c> the board has rotated to the right.</param>
        public static void RaiseBoardRotated(bool movedRight)
        {
            OnRotation?.Invoke(movedRight);
        }

        /// <summary>
        /// Invokes the action associated with board reset.
        /// </summary>
        public static void RaiseBoardReset()
        {
            OnReset?.Invoke();
        }
    }
}
