namespace Assets.Scripts.PowerUps
{
    public class AutoNextBlock : PowerUp
    {

        public override int Id => 7;

        public override string Title => "Next block";

        public override string Description => "Your current block has been switched for the next one.";

        public AutoNextBlock(GameExecuter executer) : base(executer) { }

        public override void Use()
        {
            Executer.EnqueueAction(() =>
            {
                Executer.NextWithoutPlacing();
            });
        }
    }
}
