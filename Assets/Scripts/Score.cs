using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Score
    {
        private int currentScore;
        public int CurrentScore { get { return currentScore; } set { currentScore = value; } }

        public Score()
        {
            CurrentScore = 0;
        }

        public int AddLayerScore(int level, int cleared)
        {
            int multiplier = GetMultiplier(cleared);
            CurrentScore += multiplier * (1 + level);
            return multiplier * (1 + level);
        }

        public void IncrementScore()
        {
            CurrentScore += 1;
        }

        public string GetMessage(int cleared)
        {
            switch (cleared)
            {
                case 1: return "Single";
                case 2: return "Double";
                case 3: return "Triple";
                case 4: return "Tetris";
            }
            return "";
        }

        private int GetMultiplier(int cleared)
        {
            switch (cleared)
            {
                case 1: return 400;
                case 2: return 2000;
                case 3: return 3000;
                case 4: return 12000;
            }
            return 0;
        }

    }
}