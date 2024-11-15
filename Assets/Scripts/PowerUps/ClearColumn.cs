using UnityEngine;

namespace Assets.Scripts.PowerUps
{
    public class ClearColumn : PowerUp
    {
        public override int Id => 2;
        public ClearColumn(GameExecuter executer) : base(executer) { }

        public override void Use()
        {
            Executer.ClearBlocksInColumn((int)Position.x, (int)Position.z);
        }
    }
}