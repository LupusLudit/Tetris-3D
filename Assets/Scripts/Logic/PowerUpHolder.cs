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
                new SlowDown(executer),
                new DoubleScore(executer),
                new BlockFreezer(executer)
            };
            nextPowerUp = RandomPowerUp();
        }


        //TODO: check if the power up wasn't spawned inside another block and improve current block checking (we are not looking at the actual position)
        private PowerUp RandomPowerUp()
        {
            PowerUp powerUp = powerUps[random.Next(powerUps.Length)];
            Vector3 position = Vector3.zero;
            do
            {
                position = new Vector3(random.Next(executer.XMax), random.Next(executer.YMax - 2), random.Next(executer.ZMax));
            }
            while (PositionInPlacedBlocks(position));

            powerUp.Position = position;
            return powerUp;
        }

        private bool PositionInPlacedBlocks(Vector3 position)
        {
            foreach (var block in executer.PlacedBlocks)
            {
                if (block.transform.position == position) return true;
            }

            return false;
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
