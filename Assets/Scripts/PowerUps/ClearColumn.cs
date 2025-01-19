namespace Assets.Scripts.PowerUps
{
    public class ClearColumn : PowerUp
    {
        public override int Id => 2;

        public override string Title => "Clear column";

        public override string Description => "Blocks in this column have been destroyed";

        public ClearColumn(GameExecuter executer) : base(executer) { }

        public override void Use()
        {
            Executer.Manager.ClearBlocksInColumn((int)Position.x, (int)Position.z);
        }
    }
}