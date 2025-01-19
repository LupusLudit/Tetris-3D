namespace Assets.Scripts.Logic
{
    public class GameGrid
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        private readonly int[,,] grid;


        public int this[int x, int y, int z]
        {
            get => grid[x, y, z];
            set => grid[x, y, z] = value;
        }

        public GameGrid(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            grid = new int[x,y,z];
        }


        public bool IsInBounds(int x, int y, int z)
        {
            return x >= 0 && x < X && y >= 0 && y < Y && z >= 0 && z < Z;
        }

        public bool IsEmpty(int x, int y, int z)
        {
            return IsInBounds(x, y, z) && grid[x,y,z] == 0;
        }

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

        public void ClearColumn(int x, int z)
        {
            for (int y = 0; y < Y; y++)
            {
                grid[x, y, z] = 0;
            }
        }

        public void MoveLayerDown(int y, int numOfRows)
        {
            for (int x = 0; x < X; x++)
            {
                for (int z = 0; z < Z; z++)
                {
                    grid[x, y + numOfRows, z] = grid[x, y, z];
                    grid[x, y, z] = 0;
                }
            }
        }
    }
}
