using Assets.Scripts.Logic;
using Assets.Scripts.Unity;

namespace Assets.Scripts.PowerUps
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="ClearLine"]/*'/>
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

        /// <summary>
        /// Searches from the top of the grid downward for the first non-empty row,
        /// and clears all blocks in that row once found.
        /// </summary>

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
