using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
