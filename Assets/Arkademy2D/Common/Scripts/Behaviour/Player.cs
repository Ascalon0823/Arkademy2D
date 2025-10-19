using System;
using Mirror;
using UnityEngine;

namespace Arkademy2D.Common.Behaviour
{
    public class Player : NetworkBehaviour
    {
        public Character characterPrefab;
        public Character character;

        public void Start()
        {
            if (isLocalPlayer)
            {
                CmdSpawnCharacter();
            }
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                character.transform.position += Vector3.up * 0.1f;
            }
        }

        [Command]
        public void CmdSpawnCharacter()
        {
            var newCharacter = Instantiate(characterPrefab);
            NetworkServer.Spawn(newCharacter.gameObject);
            newCharacter.netIdentity.AssignClientAuthority(connectionToClient);
            OnCharacterSpawned(newCharacter);
        }

        [ClientRpc]
        public void OnCharacterSpawned(Character identity)
        {
            character = identity;
        }
        
    }
}