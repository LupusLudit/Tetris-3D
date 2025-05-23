using Assets.Scripts.PowerUps;
using Assets.Scripts.Unity;

namespace Assets.Scripts.Logic
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="PowerUpHolder"]/*'/>
    public class PowerUpHolder
    {

        private readonly PowerUp[] powerUps;
        private GameExecuter executer;
        private readonly System.Random random = new System.Random();
        private PowerUp nextPowerUp;
        public PowerUpHolder(GameExecuter executer)
        {
            this.executer = executer;
            powerUps = new PowerUp[]
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
            nextPowerUp = RandomPowerUp();
        }

        /// <summary>
        /// Picks a random powerup.
        /// </summary>
        /// <returns>The picked powerup.</returns>
        private PowerUp RandomPowerUp()
        {
            PowerUp powerUp = powerUps[random.Next(powerUps.Length)];
            return powerUp;
        }

        /// <summary>
        /// Returns new powerup (the previously saved <see cref="nextPowerUp"/>) and generates new <see cref="nextPowerUp"/>.
        /// </summary>
        /// <returns></returns>
        public PowerUp GetNextPowerUp()
        {
            PowerUp temp = nextPowerUp;
            do
            {
                nextPowerUp = RandomPowerUp();
            }
            while (temp.Id == nextPowerUp.Id);

            return temp;
        }
    }
}
