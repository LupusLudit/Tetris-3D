using Assets.Scripts.PowerUps;
using UnityEngine;

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
                new SlowDown(executer)
            };
            nextPowerUp = RandomPowerUp();
        }


        //TODO: check if the power up wasn't spawned inside another block
        private PowerUp RandomPowerUp()
        {
            PowerUp powerUp = powerUps[random.Next(powerUps.Length)];
            powerUp.Position = new Vector3(random.Next(executer.XMax), random.Next(executer.YMax - 2), random.Next(executer.ZMax));
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
