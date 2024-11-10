using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public abstract class PowerUp
    {
        protected Vector3 Position { get; set; }

        protected PowerUp(Vector3 position)
        {
            Position = position;
        }

        public abstract void use();
    }
}
