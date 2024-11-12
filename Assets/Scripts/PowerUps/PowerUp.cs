using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public abstract class PowerUp
    {
        protected Vector3 Position { get; }
        protected GameExecuter Executer { get; }

        public PowerUp(Vector3 position, GameExecuter executer)
        {
            Position = position;
            Executer = executer;
        }

        public abstract void Use();
    }
}
