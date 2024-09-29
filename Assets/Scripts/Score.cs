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

        public void AddLayerScore(int level, int cleared)
        {
            int multiplier = GetMultiplier(cleared);
            CurrentScore += multiplier*(1+level);
        }

        public void IncrementScore()
        {
            CurrentScore += 1;
        }

        private int GetMultiplier(int cleared)
        {
            switch (cleared)
            {
                case 1: return 40;
                case 2: return 100;
                case 3: return 300;
                case 4: return 1200;
            }
            return 0;
        }


    }
}
