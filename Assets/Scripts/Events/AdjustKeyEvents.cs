using System;

namespace Assets.Scripts.Events
{
    public static class AdjustKeyEvents
    {
        public static event Action<bool> OnRotation;
        public static event Action OnReset;
        public static void RaiseBoardRotated(bool movedRight)
        {
            OnRotation?.Invoke(movedRight);
        }

        public static void RaiseBoardReset()
        {
            OnReset?.Invoke();
        }
    }
}
