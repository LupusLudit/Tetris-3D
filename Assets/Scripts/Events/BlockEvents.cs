using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public static class BlockEvents
    {
        public static event Action<List<Vector3>> OnBlockPlaced;

        public static void RaiseBlockPlaced(List<Vector3> placedPositions)
        {
            OnBlockPlaced?.Invoke(placedPositions);
        }
    }

}
