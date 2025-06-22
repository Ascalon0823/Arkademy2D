using System;
using Midterm.Character;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Midterm.Field
{
    public class Enemy : MonoBehaviour
    {
        public Character.Character character;

        public int xp;
        public XPPickup xpPickupPrefab;
        public int energy;
        public EnergyPickup energyPickupPrefab;

        private void Start()
        {
            character.onDead.AddListener(() =>
            {
                var xpPickup = Instantiate(xpPickupPrefab, transform.position + (Vector3)Random.insideUnitCircle,
                    Quaternion.identity);
                xpPickup.xp = xp;
                var energyPickup = Instantiate(energyPickupPrefab,
                    transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
                energyPickup.energy = energy;
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