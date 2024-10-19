using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkademy.Behaviour.UI
{
    public class NamedPages : MonoBehaviour
    {
        public List<NamedPage> pages = new List<NamedPage>();
        public int currIdx;

        private void Start()
        {
            Toggle(false);
        }

        public void Toggle(bool active)
        {
            Game.RequirePause(active);
            gameObject.SetActive(active);
            UpdatePage(0);
        }

        public void UpdatePage(int diff)
        {
            currIdx += diff;
            currIdx = currIdx < 0 ? pages.Count - 1 : currIdx % pages.Count;
            foreach (var page in pages)
            {
                page.Activate(pages[currIdx].key);
            }
        }
    }
}