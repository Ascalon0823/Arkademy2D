using System;
using Mirror;
using UnityEngine;

namespace Arkademy2D.Common.Behaviour
{
    public class Player : NetworkBehaviour
    {
        public Character characterPrefab;
        public Character character;
        public Camera playerCameraPrefab;
        public Camera playerCamera;
        
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
            if (!character) return;
            var dir = Vector2.zero;
            if (Input.GetKey(KeyCode.W)) dir += Vector2.up;
            if (Input.GetKey(KeyCode.S)) dir += Vector2.down;
            if (Input.GetKey(KeyCode.A)) dir += Vector2.left;
            if (Input.GetKey(KeyCode.D)) dir += Vector2.right;
            character.moveDir = dir;

            if (!playerCamera) return;
            playerCamera.transform.position = character.transform.position + new Vector3(0, 0, -10);
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
            if (isOwned)
            {
                playerCamera = Instantiate(playerCameraPrefab, transform, true);
            }
        }
    }
}