using Assets.Scripts.PowerUps;
using Assets.Scripts.Unity;

namespace Assets.Scripts.Logic.Holders
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="PowerUpHolder"]/*'/>
    public class PowerUpHolder: BaseHolder<PowerUp>
    {

        private readonly GameExecuter executer;

        public PowerUpHolder(GameExecuter executer)
        {
            this.executer = executer;
            InitializeHolder();
        }

        protected override PowerUp[] InitializeItems() =>
            new PowerUp[]
            {
                new ClearColumn(executer),
                new ClearLine(executer),
                new SlowDown(executer),
                new DoubleScore(executer),
                new BlockFreezer(executer),
                new AutoNextBlock(executer),
                new SpeedUp(executer),
                new BlindPlayer(executer),
                new InstaDrop(executer),
                new LimitMovement(executer)
            };

        protected override int GetId(PowerUp item) => item.Id;

        public PowerUp NextPowerUp => NextItem;
        public PowerUp GetNextPowerUp() => GetNewNextItem();
    }
}
