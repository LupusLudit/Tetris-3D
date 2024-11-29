using Assets.Scripts.PowerUps;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviour
{
    public class PositionConvertor
    {
        public static Vector3 PowerUpPosition(PowerUp powerUp, Renderer renderer, int maxY)
        {
            Vector3 v = powerUp.Position;
            Vector3 cubeSize = renderer.bounds.size;
            return new Vector3(v.x + cubeSize.x / 2, maxY - 3 - v.y + cubeSize.y / 2, v.z + cubeSize.z / 2);
        }

        public static Vector3 ActualTilePosition(Vector3 v, Renderer renderer, int maxY)
        {
            Vector3 cubeSize = renderer.bounds.size;
            return new Vector3(v.x + cubeSize.x / 2, maxY - 1 - v.y + cubeSize.y / 2, v.z + cubeSize.z / 2);
        }
    }
}
