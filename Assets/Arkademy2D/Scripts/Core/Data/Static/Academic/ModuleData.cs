using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkademy2D.Core.Data.Static.Academic
{
    [CreateAssetMenu(fileName = "Module", menuName = ModuleData.ModuleResourcesPath)]
    public class ModuleData : ScriptableObject
    {
        private const string ModuleResourcesPath = "Static/Academic/Modules"; 
        public static List<ModuleData> Modules
        {
            get
            {
                if (_modules == null)
                {
                    _modules = LoadAll();
                }
                return _modules;
            }
        }
        private static List<ModuleData> _modules;
        private static List<ModuleData> LoadAll()
        {
            return Resources.LoadAll<ModuleData>(ModuleResourcesPath).ToList();
        }

        [SerializeField] private string id;
        public string Id => id;
        public string displayName;
        public List<ModuleData> prerequisites = new List<ModuleData>();
    }
}