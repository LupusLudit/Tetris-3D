using System;
using UnityEngine;

namespace Assets.Scripts.MonoBehavior
{
    public class BoardDimensions
    {

        public static Vector3 calcLookPoint(Vector3 boardDim, Camera camera)
        {
            double cameraY = camera.transform.position.y;
            double angleDown = camera.transform .rotation.eulerAngles.x * (Math.PI / 180);
            //distance from point 0,0,0
            double x = camera.transform.position.x;
            double z = camera.transform.position.z;
            double distance = Math.Sqrt(x * x + z * z) - Math.Sqrt(boardDim.x/2 * boardDim.x/2 + boardDim.z/2 * boardDim.z/2);
            //calculating y distance from look point to the camera
            double yDelta = Math.Tan(angleDown) * distance;
            double actualY = cameraY - yDelta;

            return new Vector3(boardDim.x/2, (float)actualY, boardDim.z/2);
        }

        public static Vector3[] calcHitPoints(Vector3 boardDim, Camera camera)
        {
            float x = camera.transform.position.x;
            float z = camera.transform.position.z;
            float y = camera.transform.position.y;

            //Radius of the circle the camera moves on
            float radius = (float) (Math.Sqrt(x * x + z * z) - Math.Sqrt(boardDim.x / 2 * boardDim.x / 2 + boardDim.z / 2 * boardDim.z / 2));

            return new Vector3[]
            {
                new Vector3(radius + boardDim.x/2, y, boardDim.z/2),
                new Vector3(boardDim.x/2, y, radius + boardDim.z/2),
                new Vector3(-radius + boardDim.x/2, y, boardDim.z/2),
                new Vector3(boardDim.x/2, y, -radius + boardDim.z/2)
            };
        }
    }
}
