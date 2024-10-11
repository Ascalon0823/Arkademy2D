using System;
using Arkademy;
using Arkademy.Data;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

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

    [CustomPropertyDrawer(typeof(AffixData.TargetCategories))]
    public class TargetCategoriesCustomDrawer : PropertyDrawer
    {
        private UnityEngine.Object target;

        private void OnEditorUpdate()
        {
            EditorApplication.update -= OnEditorUpdate;
            Selection.activeObject = target;
        }

        public VisualElement GetChildElement(SerializedProperty categoryValue, AffixData.Category actualCategory,
            SerializedProperty property)
        {
            EnumField childEnumField = null;
            var actualValue = categoryValue.intValue;

            switch (actualCategory)
            {
                case AffixData.Category.CharacterAttrBoost:
                    childEnumField = new EnumField("Character Attr", (CharacterData.Attr.Category)actualValue);
                    break;
                case AffixData.Category.MasteryBoost:
                    childEnumField = new EnumField("Equipment Type", (EquipmentData.Type)actualValue);
                    break;
                case AffixData.Category.EquipmentAttrBoost:
                    childEnumField = new EnumField("Equipment Attr", (EquipmentData.Attr.Category)actualValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            childEnumField.RegisterValueChangedCallback(e =>
            {
                categoryValue.SetInline(Convert.ToInt32(e.newValue));
                property.serializedObject.ApplyModifiedProperties();
            });
            return childEnumField;
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var category = property.FindPropertyRelative("category");
            var categoryValue = property.FindPropertyRelative("categoryValue");
            var container = new VisualElement();
            var actualCategory = category.GetEnumValue<AffixData.Category>();
            var enumField = new EnumField("Affix Category", actualCategory);
            container.Add(enumField);
            enumField.RegisterValueChangedCallback(e =>
            {
                category.SetEnumValue((AffixData.Category)e.newValue);
                property.serializedObject.ApplyModifiedProperties();
                container.RemoveAt(1);
                container.Add(GetChildElement(categoryValue, (AffixData.Category)e.newValue, property));
            });
            container.Add(GetChildElement(categoryValue, actualCategory, property));
            return container;
        }
    }
}