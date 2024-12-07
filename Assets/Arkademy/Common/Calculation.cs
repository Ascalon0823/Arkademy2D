using UnityEngine;

namespace Arkademy.Common
{
    public static class Calculation
    {
        public static float MoveSpeed(int moveSpeed)
        {
            return moveSpeed / 100f;
        }

        public static float CastCoolDown(int castSpeed)
        {
            return  100f/Mathf.Max(1,castSpeed);
        }

        public static int Damage(int attack)
        {
            return Mathf.FloorToInt(attack / 100f);
        }
        public static int DamageTaken(int damage, int defence)
        {
            return Mathf.FloorToInt(damage / (Mathf.Max(1, defence) / 100f));
        }
    }
}