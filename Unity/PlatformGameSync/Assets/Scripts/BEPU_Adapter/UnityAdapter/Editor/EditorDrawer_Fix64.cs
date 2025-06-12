#if UNITY_EDITOR
using FixMath.NET;
using UnityEditor;
using UnityEngine;


namespace BEPU_Adapter.UnityAdapter {
    [CustomPropertyDrawer(typeof(Fix64))]
    public class EditorDrawer_Fix64 : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label.text = $"(Fix64) {property.name}";
            float floatV = (float)((Fix64)property.boxedValue);
            float newValue = EditorGUI.FloatField(position, label, floatV);
            property.boxedValue = ((Fix64)newValue) as object;

            Fix64 f1 = (Fix64)0.5f;
        }
    }
}
#endif