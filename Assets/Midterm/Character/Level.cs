using UnityEngine;

namespace Midterm.Character
{
    public class Level : MonoBehaviour
    {
        public int totalXp;

        public void AddXp(int xp)
        {
            totalXp += xp;
        }
    }
}