using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkademy.UI.Title
{
    public class TileMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playCount;
        
        private void Start()
        {
            playCount.text = $"You have played {SaveData.current.numOfGamesPlayed} times";
        }
    }
}
