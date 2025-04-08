using Assets.Scripts.PowerUps;
using Assets.Scripts.Unity;

namespace Assets.Scripts.Logic
{
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
                new Bomb(executer),
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


        //TODO: check if the power up wasn't spawned inside another block and improve current block checking (we are not looking at the actual position)
        private PowerUp RandomPowerUp()
        {
            PowerUp powerUp = powerUps[random.Next(powerUps.Length)];
            return powerUp;
        }

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
