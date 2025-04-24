namespace Assets.Scripts.Logic
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="GameGrid"]/*'/>
    public class GameGrid
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        private readonly int[,,] grid;

        /*
         *   y
         *   |
         *   |
         *   |_____z
         *  /
         * /
         * x
         */

        /// <summary>
        /// Gets or sets the value at a specific (x, y, z) coordinate in the grid.
        /// </summary>
        public int this[int x, int y, int z]
        {
            get => grid[x, y, z];
            set => grid[x, y, z] = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameGrid"/> class with the specified dimensions.
        /// </summary>
        /// <param name="x">Width of the grid.</param>
        /// <param name="y">Height of the grid.</param>
        /// <param name="z">Depth of the grid.</param>
        public GameGrid(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            grid = new int[x,y,z];
        }

        /// <summary>
        /// Checks if the given (x, y, z) coordinate is within the bounds of the grid.
        /// </summary>
        public bool IsInBounds(int x, int y, int z)
        {
            return x >= 0 && x < X && y >= 0 && y < Y && z >= 0 && z < Z;
        }

        /// <summary>
        /// Returns true if the specified cell is within bounds and currently empty (value is 0).
        /// </summary>
        public bool IsEmpty(int x, int y, int z)
        {
            return IsInBounds(x, y, z) && grid[x,y,z] == 0;
        }

        /// <summary>
        /// Checks whether every tile in the specified horizontal layer (Y level) is occupied.
        /// </summary>
        public bool IsLayerFull(int y)
        {
            for (int x = 0; x < X; x++)
            {
                for (int z = 0; z < Z; z++)
                {
                    if (grid[x, y, z] == 0) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks whether every tile in the specified horizontal layer (Y level) is empty.
        /// </summary>
        public bool IsLayerEmpty(int y)
        {
            for (int x = 0; x < X; x++)
            {
                for (int z = 0; z < Z; z++)
                {
                    if (grid[x, y, z] != 0) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Clears all tiles in the specified layer by setting their values to 0.
        /// </summary>
        public void ClearLayer(int y)
        {
            for (int x = 0; x < X; x++)
            {
                for (int z = 0; z < Z; z++)
                {
                    grid[x, y, z] = 0;
                }
            }
        }

        /// <summary>
        /// Clears all tiles in a vertical column specified by (x, z) by setting their values to 0.
        /// </summary>
        public void ClearColumn(int x, int z)
        {
            for (int y = 0; y < Y; y++)
            {
                grid[x, y, z] = 0;
            }
        }

        /// <summary>
        /// Moves all tiles in the specified layer down by the given number of rows.
        /// Used after clearing layers to shift everything above downward.
        /// </summary>
        /// <param name="y">The Y coordinate of the layer to move.</param>
        /// <param name="numOfRows">How many rows to move the layer down by.</param>
        public void MoveLayerDown(int y, int numOfRows)
        {
            for (int x = 0; x < X; x++)
            {
                for (int z = 0; z < Z; z++)
                {
                    grid[x, y - numOfRows, z] = grid[x, y, z];
                    grid[x, y, z] = 0;
                }
            }
        }
    }
}
