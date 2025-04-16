using UnityEngine;

namespace Arkademy.UI
{
    public class AbilityPage : Page
    {
        public AbilitySlotItem[] items;
        public override void OnShow()
        {
            base.OnShow();
            foreach (var item in items)
            {
                item.Init();
            }
        }
    }
}