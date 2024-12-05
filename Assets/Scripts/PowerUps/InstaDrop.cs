namespace Assets.Scripts.PowerUps
{
    public class InstaDrop : PowerUp
    {
        public override int Id => 10;
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
