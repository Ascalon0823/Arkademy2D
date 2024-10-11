using Arkademy.Data;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(EquipmentBase))]
    public class EquipmentBaseCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Test Roll"))
            {
                Debug.Log(JsonConvert.SerializeObject((target as EquipmentBase).Generate(), Formatting.Indented,
                    new Newtonsoft.Json.Converters.StringEnumConverter()));
            }
        }
    }
}