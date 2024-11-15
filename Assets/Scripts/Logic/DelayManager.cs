using System;

namespace Assets.Scripts.Logic
{
    public class DelayManager
    {

        private int maxDelay;
        private int minDelay;
        private int delayDecrease;

        public int CurrentDelay { get; set; }

        public DelayManager(int maxDelay, int minDelay, int delayDecrease)
        {
            this.maxDelay = maxDelay;
            this.minDelay = minDelay;
            this.delayDecrease = delayDecrease;
            CurrentDelay = maxDelay;
        }

        public void AdjustDelay(int score, bool slowDown)
        {
            int temp = Math.Max(minDelay, maxDelay - (score / 10000 * delayDecrease));
            if(!slowDown) CurrentDelay = temp;
            else CurrentDelay = temp * 2;
        }
    }
}
