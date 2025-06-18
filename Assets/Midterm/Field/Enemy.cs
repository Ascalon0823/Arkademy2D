using System;
using Midterm.Character;
using UnityEngine;

namespace Midterm.Field
{
    public class Enemy : MonoBehaviour
    {
        public Character.Character character;

        public int xp;
        public XPPickup xpPickupPrefab;
        private void Start()
        {
            character.onDead.AddListener(() =>
            {
                var xpPickup=Instantiate(xpPickupPrefab,transform.position,Quaternion.identity);
                xpPickup.xp = xp;
            });
        }

        public void FixedUpdate()
        {
            var playerChar = Player.Player.Local.currCharacter;
            var moveDir = playerChar.transform.position - transform.position;
            character.moveDir = moveDir.normalized;
        }
    }
}