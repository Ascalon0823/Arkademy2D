using System;
using System.Collections.Generic;
using Arkademy2D.Game.Data.Static.Item;

namespace Arkademy2D.Game.Data.Runtime
{
    [Serializable]
    public class ItemData
    {
        public ItemBase itemBase;
        public List<UsableData> usableData;

        public void Update(float deltaTime)
        {
            if (usableData?.Count == 0) return;
            foreach (var usable in usableData)
            {
                usable.Update(deltaTime);
            }
        }
    }
}