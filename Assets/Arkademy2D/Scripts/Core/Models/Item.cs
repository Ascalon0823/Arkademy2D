using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Arkademy2D.Core.Models
{
    public class Item
    {
        public int ItemBaseId { get; set; }
        public ItemExtras Extras { get; set; } = new ItemExtras();
    }

    public abstract class ItemExtra
    {
    }
    public class ItemExtras
    {
        public Dictionary<string, JObject> ExtrasDictionary = new Dictionary<string, JObject>();

        public bool TryGetExtra<T>(string key, out T itemExtra) where T : ItemExtra
        {
            if (ExtrasDictionary.TryGetValue(key, out var extra) )
            {
                itemExtra = JsonConvert.DeserializeObject<T>(extra.ToString());
                return true;
            }
            itemExtra = null;
            return false;
        }

        public void AddExtra<T>(string key, T itemExtra) where T : ItemExtra
        {
            ExtrasDictionary[key] = JObject.FromObject(itemExtra);
        }
    }
}