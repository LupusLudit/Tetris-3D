using Assets.Scripts.Unity;

namespace Assets.Scripts.PowerUps
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="ClearColumn"]/*'/>
    public class ClearColumn : PowerUp
    {
        public override int Id => 2;

        public override string Title => "Clear column";

        public override string Description => "Blocks in this column have been destroyed";

        public ClearColumn(GameExecuter executer) : base(executer) { }

        /// <summary>
        /// Calls the game manager to remove all blocks in the column corresponding 
        /// to the powerup's current x and z position.
        /// </summary>
        public override void Use()
        {
            Executer.Manager.ClearBlocksInColumn((int)Position.x, (int)Position.z);
        }
    }
}