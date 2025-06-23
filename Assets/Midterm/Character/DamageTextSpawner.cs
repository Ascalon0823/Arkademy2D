using System.Collections;
using Midterm.Player;
using UnityEngine;

namespace Midterm.Character
{
    public class DamageTextSpawner : MonoBehaviour
    {
        public void SpawnDamage(int[] damages)
        {
            Player.Player.Local.SpawnDamageText(transform,damages);
        }
    }
}