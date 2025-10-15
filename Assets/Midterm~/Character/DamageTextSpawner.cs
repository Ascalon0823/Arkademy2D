using System.Collections;
using Midterm.Player;
using UnityEngine;

namespace Midterm.Character
{
    public class DamageTextSpawner : MonoBehaviour
    {
        public void SpawnDamage(int damage,int group = -1)
        {
            Player.Player.Local.SpawnDamageText(transform,damage,group);
        }
    }
}