using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Events
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="BlockEvents"]/*'/>
    public static class BlockEvents
    {
        public static event Action<List<Vector3>> OnBlockPlaced;

        /// <summary>
        /// Invokes the action associated with placing of the block.
        /// </summary>
        /// <param name="placedPositions">The positions of the placed block.</param>
        public static void RaiseBlockPlaced(List<Vector3> placedPositions)
        {
            OnBlockPlaced?.Invoke(placedPositions);
        }
    }

}
