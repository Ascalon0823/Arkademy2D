using System;
using Arkademy.Templates;
using UnityEngine;

namespace Arkademy.Behaviour
{
   public class CharacterSpawner : MonoBehaviour
   {
      public Character prefab;
      public CharacterTemplate template;

      public bool spawnOnStart;
      public Character lastSpawnedCharacter;
      public int faction;
      private void Start()
      {
         if (spawnOnStart)
         {
            Spawn();
         }
      }

      public virtual void Spawn()
      {
         lastSpawnedCharacter = Instantiate(prefab,transform.position,Quaternion.identity);
         lastSpawnedCharacter.Setup(template.GetNewCharacter(),faction);
      }
   }
}
