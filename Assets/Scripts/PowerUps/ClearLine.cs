namespace Assets.Scripts.PowerUps
{
    public class ClearLine : PowerUp
    {

        private GameGrid grid;
        public override int Id => 1;
        public ClearLine(GameExecuter executer) : base(executer)
        {
            grid = executer.CurrentGame.Grid;
        }

        public override void Use()
        {
            int maxY = Executer.YMax;
            for (int i = 0; i < maxY; i++)
            {
                if (!grid.IsLayerEmpty(i))
                {
                    Executer.ClearBlocksInRow(i);
                    break;
                }
            }
        }
    }
}
