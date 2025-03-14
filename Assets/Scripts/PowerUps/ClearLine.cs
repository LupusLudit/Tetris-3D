using Assets.Scripts.Logic;

namespace Assets.Scripts.PowerUps
{
    public class ClearLine : PowerUp
    {

        private GameGrid grid;
        public override int Id => 1;

        public override string Title => "Clear line";

        public override string Description => "The top line has been destroyed";

        public ClearLine(GameExecuter executer) : base(executer)
        {
            grid = executer.CurrentGame.Grid;
        }

        public override void Use()
        {
            int maxY = Executer.YMax;
            for (int i = maxY-1; i >= 0; i--)
            {
                if (!grid.IsLayerEmpty(i))
                {
                    Executer.Manager.ClearBlocksInRow(i);
                    break;
                }
            }
        }
    }
}
