namespace Assets.Scripts.Logic
{
    /// <include file='../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="Score"]/*'/>
    public class Score
    {
        private int currentScore;
        public int CurrentScore { get { return currentScore; } set { currentScore = value; } }

        public Score()
        {
            CurrentScore = 0;
        }

        /// <summary>
        /// Adds score based on the number of cleared layers, the current level, and whether double score is active.
        /// </summary>
        /// <param name="level">The current level of the game, which increases the score multiplier.</param>
        /// <param name="cleared">The number of layers cleared in a single move.</param>
        /// <param name="doubleScore">If true, the earned score is doubled.</param>
        /// <returns>The amount of score added for the cleared layers.</returns>
        public int AddLayerScore(int level, int cleared, bool doubleScore)
        {
            int multiplier = GetMultiplier(cleared);
            int plusScore = multiplier * (1 + level);
            if (doubleScore) plusScore *= 2;

            CurrentScore += plusScore;
            return plusScore;
        }

        /// <summary>
        /// Increments the score by 1 or 2 depending on whether double score is active.
        /// </summary>
        /// <param name="doubleScore">If true, increments by 2; otherwise, by 1.</param>
        public void IncrementScore(bool doubleScore)
        {
            if (doubleScore) CurrentScore += 2;
            else CurrentScore += 1;
        }

        /// <summary>
        /// Returns a descriptive message based on the number of layers cleared.
        /// </summary>
        /// <param name="cleared">The number of layers cleared in a single move.</param>
        /// <returns>A string representing the type of clear (e.g., "Single", "Double", etc.).</returns>
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

        /// <summary>
        /// Returns the score multiplier based on the number of layers cleared.
        /// </summary>
        /// <param name="cleared">The number of layers cleared.</param>
        /// <returns>The score multiplier associated with the given number of cleared layers.</returns>
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