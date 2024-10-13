using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Behaviour
{
    public class Player : MonoBehaviour
    {
        public PlayerRecord playerRecord;
        public bool local;
        public Character controllingCharacter;
        public PixelPerfectCamera characterCamera;
        
    }
}