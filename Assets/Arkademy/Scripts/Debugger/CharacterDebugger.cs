using System;
using System.Collections.Generic;
using System.Linq;
using Arkademy.Data;
using UnityEngine;

namespace Arkademy.Debugger
{
    [RequireComponent(typeof(Gameplay.Character))]
    public class CharacterDebugger : MonoBehaviour
    {
        [SerializeField]private Gameplay.Character character; 
        [SerializeField]private List<AttrDisplay> displays = new ();
        private void Start()
        {
            if(!Application.isEditor)Destroy(this);
            displays = character.data.attributes.Select(x => new AttrDisplay(x.Value)).ToList();
        }

        private void LateUpdate()
        {
            foreach (var display in displays)
            {
                display.Update();
            }
        }
    }
}