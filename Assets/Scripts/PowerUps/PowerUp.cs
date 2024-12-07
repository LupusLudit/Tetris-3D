using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public abstract class PowerUp
    {
        public Vector3 Position { get; set; }
        public abstract int Id { get; }
        public abstract string Title { get; }
        public abstract string Description { get; }
        protected GameExecuter Executer { get; }

        public PowerUp(GameExecuter executer)
        {
            Executer = executer;
        }

        public abstract void Use();
    }
}

