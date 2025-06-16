using System;
using System.Collections.Generic;
using UnityEngine;
namespace Arkademy.Data.Scriptable
{
    [CreateAssetMenu(fileName = "Race", menuName = "Data/Race", order = 0)]
    public class RaceObject : ScriptableObject
    {
        public Race race;
        [SerializeField] private List<AttrDisplay> displays = new();

        private void OnValidate()
        {
            displays.Clear();
            foreach (var attr in race.attributes)
            {
                var ad = new AttrDisplay(attr.Copy());
                displays.Add(ad);
            }
        }
    }
}