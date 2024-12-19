using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Arkademy.Common
{
    [Serializable]
    public class ChanceTable<T>
    {
        [Serializable]
        public class Record
        {
            public float chance;
            public T item;
        }
        
        public List<Record> records = new List<Record>();

        public virtual T Roll()
        {
            var v = Random.Range(0f, 1f);
            foreach (var record in records.OrderBy(x=>x.chance))
            {
                if (v < record.chance)
                {
                    return record.item;
                }
            }
            return default;
        }
    }
}