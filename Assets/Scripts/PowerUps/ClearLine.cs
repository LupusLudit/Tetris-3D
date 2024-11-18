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
            int y = Executer.YMax;
            for (int i = y; i > 0; i--)
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
