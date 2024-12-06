using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.PowerUps
{
    public class AutoNextBlock : PowerUp
    {

        public override int Id => 7;
        public AutoNextBlock(GameExecuter executer) : base(executer) { }

        public override void Use()
        {
            Executer.EnqueueAction(() =>
            {
                Executer.NextWithoutPlacing();
            });
        }
    }
}
