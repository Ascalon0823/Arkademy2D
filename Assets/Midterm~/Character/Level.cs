using UnityEngine;
using UnityEngine.Events;

namespace Midterm.Character
{
    public class Level : MonoBehaviour
    {
        public int totalXp;
        public int lastXp;
        public int incrementXp;
        public int nextXp;
        public int currLevel;
        public UnityEvent<int> onLevelUp;
        public void AddXp(int xp)
        {
            totalXp += xp;
            while (totalXp >= nextXp)
            {
                LevelUp();    
            }
        }

        private void LevelUp()
        {
            currLevel++;
            lastXp = nextXp;
            incrementXp +=  currLevel <= 2 ? 5 : currLevel<=10 ? 15 : 25;
            nextXp += incrementXp;
            onLevelUp?.Invoke(currLevel);
        }

        public float GetXPPercentage()
        {
            return (totalXp - lastXp)*1f/(nextXp-lastXp);
        }
    }
}