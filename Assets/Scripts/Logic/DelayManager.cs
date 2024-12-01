using System;

namespace Assets.Scripts.Logic
{
    public class DelayManager
    {

        private int maxDelay;
        private int minDelay;
        private int delayDecrease;

        public double CurrentDelay { get; set; }

        public DelayManager(int maxDelay, int minDelay, int delayDecrease)
        {
            this.maxDelay = maxDelay;
            this.minDelay = minDelay;
            this.delayDecrease = delayDecrease;
            CurrentDelay = maxDelay;
        }

        public void AdjustDelay(int score, double multiplier)
        {
            int temp = Math.Max(minDelay, maxDelay - (score / 10000 * delayDecrease));
            CurrentDelay = temp * multiplier;
        }
    }
}
