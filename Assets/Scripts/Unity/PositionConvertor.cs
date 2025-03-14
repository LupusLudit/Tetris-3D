using Assets.Scripts.PowerUps;
using UnityEngine;
public class PositionConvertor
{
    public static Vector3 PowerUpPosition(PowerUp powerUp, GameExecuter executer, int maxY)
    {
        Vector3 v = powerUp.Position;
        return new Vector3(v.x, maxY - 3 - v.y, v.z);
    }

    public static Vector3 ActualTilePosition(Vector3 v, GameExecuter executer, int maxY)
    {
        return new Vector3(v.x, maxY - 1 - v.y, v.z);
    }
}
