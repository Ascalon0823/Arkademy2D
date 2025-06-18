using System;
using ArkademyStudio.PixelPerfect;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Midterm.Player
{
    [Serializable]
    public class PlayerSaveData
    {
        public int playCount;
    }
    public class Player : MonoBehaviour
    {
        public static Player Local;
        public PlayerSaveData record;
        public Vector2 playerMove;
        public Character.Character currCharacter;
        public PixelPerfectCamera currCamera;

        private void Awake()
        {
            Local = this;
            var loaded = SaveEngine.Instance.Load();
            if (loaded == null)
            {
                loaded = new PlayerSaveData();
                SaveEngine.Instance.Save(loaded);
            }
            record = loaded;
        }

        public void OnMove(InputValue value)
        {
            playerMove = value.Get<Vector2>();
        }

        private void Update()
        {
            currCharacter.moveDir = playerMove;
        }
    }
}