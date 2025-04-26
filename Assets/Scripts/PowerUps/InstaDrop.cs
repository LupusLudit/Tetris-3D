using Assets.Scripts.Unity;

namespace Assets.Scripts.PowerUps
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="InstaDrop"]/*'/>
    public class InstaDrop : PowerUp
    {
        public override int Id => 10;

        public override string Title => "Instant drop";

        public override string Description => "The current block has been dropped";

        public InstaDrop(GameExecuter executer) : base(executer) { }

        /// <summary>
        /// Queues an action that forces the current block to drop to the bottom and then restarts with a new block.
        /// </summary>

        public override void Use()
        {
            Executer.EnqueueAction(() =>
            {
                Executer.DropAndRestart();
            });
        }
    }
}
