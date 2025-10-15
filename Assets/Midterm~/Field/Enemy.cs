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
        public float energyDropRate;
        public EnergyPickup energyPickupPrefab;

        public bool autoDespawn;
        public float remainingTime;
        public bool fixedMove;
        public Vector2 fixMovingDir;
        private void Start()
        {
            character.onDead.AddListener(() =>
            {
                var xpPickup = Instantiate(xpPickupPrefab, transform.position + (Vector3)Random.insideUnitCircle,
                    Quaternion.identity);
                xpPickup.xp = xp;
                if (Random.Range(0f, 1f) < energyDropRate)
                {
                    var energyPickup = Instantiate(energyPickupPrefab,
                        transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
                    energyPickup.energy = energy;
                }
            });
        }

        public void FixedUpdate()
        {
            if (autoDespawn)
            {
                remainingTime -= Time.fixedDeltaTime;
                if (remainingTime < 0)
                {
                    Destroy(gameObject);
                    return;
                }
            }
            
            var playerChar = Player.Player.Local.currCharacter;
            var moveDir = playerChar.transform.position - transform.position;
            character.moveDir = fixedMove ? fixMovingDir : moveDir.normalized;
        }
    }
}