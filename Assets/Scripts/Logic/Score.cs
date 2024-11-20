using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

        public int AddLayerScore(int level, int cleared, bool doubleScore)
        {
            int multiplier = GetMultiplier(cleared);
            int plusScore = multiplier * (1 + level);
            if (doubleScore) plusScore *= 2;

            CurrentScore += plusScore;
            return plusScore;
        }

        public void IncrementScore(bool doubleScore)
        {
            if (doubleScore) CurrentScore += 2;
            else CurrentScore += 1;
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