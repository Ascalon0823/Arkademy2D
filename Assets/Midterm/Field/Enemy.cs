using System;
using UnityEngine;

namespace Midterm.Field
{
    public class Enemy : MonoBehaviour
    {
        public Character.Character character;

        public void FixedUpdate()
        {
            var playerChar = Player.Player.Local.currCharacter;
            var moveDir = playerChar.transform.position - transform.position;
            character.moveDir = moveDir.normalized;
        }
    }
}