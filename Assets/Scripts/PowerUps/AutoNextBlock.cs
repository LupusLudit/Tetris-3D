using Assets.Scripts.Unity;

namespace Assets.Scripts.PowerUps
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="AutoNextBlock"]/*'/>
    public class AutoNextBlock : PowerUp
    {

        public override int Id => 7;

        public override string Title => "Next block";

        public override string Description => "Your current block has been switched for the next one.";

        public AutoNextBlock(GameExecuter executer) : base(executer) { }

        /// <summary>
        /// Enqueues the action to switch the current block for the next one in game manager.
        /// </summary>
        public override void Use()
        {
            Executer.EnqueueAction(() =>
            {
                Executer.Manager.NextWithoutPlacing();
            });
        }
    }
}
