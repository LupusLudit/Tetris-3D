using System;

namespace Assets.Scripts.Logic.Holders
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="BaseHolder"]/*'/>
    public abstract class BaseHolder<T>
    {
        // Note: Commentary for this class implies for all holder classes that override it as well.

        protected T[] Items;
        protected T NextItem;

        private readonly Random random = new Random();

        /// <summary>
        /// Initializes the instance of the holder.
        /// These methods can not be called from the constructor because certain
        /// elements have to be set first, before calling the InitializeItems method.
        /// </summary>
        protected void InitializeHolder()
        {
            Items = InitializeItems();
            NextItem = RandomItem();
        }

        /// <summary>
        /// Implement to initialize item array.
        /// </summary>
        /// <returns>The array of the items.</returns>
        protected abstract T[] InitializeItems();

        /// <summary>
        /// Get the identifier for comparison.
        /// </summary>
        /// <returns>The id of the item.</returns>
        protected abstract int GetId(T item);

        /// <summary>
        /// Picks a random item.
        /// </summary>
        /// <returns>The picked block.</returns>
        protected T RandomItem() =>
            Items[random.Next(Items.Length)];


        /// <summary>
        /// Returns a new item (the previously saved <see cref="NextItem"/>) and generates a new <see cref="NextItem"/>.
        /// </summary>
        /// <returns></returns>
        public T GetNewNextItem()
        {
            T current = NextItem;
            do
            {
                NextItem = RandomItem();
            }
            while (GetId(current) == GetId(NextItem));

            return current;
        }
    }
}
