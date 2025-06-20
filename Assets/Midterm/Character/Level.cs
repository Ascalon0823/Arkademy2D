using UnityEngine;
using UnityEngine.Events;

namespace Midterm.Character
{
    public class Level : MonoBehaviour
    {
        public int totalXp;
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
            nextXp += 5;
            onLevelUp?.Invoke(currLevel);
        }
    }
}