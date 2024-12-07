namespace Assets.Scripts.PowerUps
{
    public class InstaDrop : PowerUp
    {
        public override int Id => 10;

        public override string Title => "Instant drop";

        public override string Description => "The current block has been dropped";

        public InstaDrop(GameExecuter executer) : base(executer) { }

        public override void Use()
        {
            Executer.EnqueueAction(() =>
            {
                Executer.DropAndRestart();
            });
        }
    }
}
