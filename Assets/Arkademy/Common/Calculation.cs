using UnityEngine;

namespace Arkademy.Common
{
    public static class Calculation
    {
        public static float MoveSpeed(int moveSpeed)
        {
            return moveSpeed / 100f * 4f;
        }

        public static float CastCoolDown(int castSpeed)
        {
            return castSpeed / 100f;
        }

        public static int DamageTaken(int damage, int defence)
        {
            return Mathf.FloorToInt(damage / (Mathf.Max(1, defence) / 100f));
        }
    }
}