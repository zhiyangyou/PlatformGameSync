#if UNITY_EDITOR
using FixMath.NET;
using UnityEditor;
using UnityEngine;
using FVector2 = BEPUutilities.Vector2;


namespace BEPU_Adapter.UnityAdapter {
    [CustomPropertyDrawer(typeof(FVector2))]
    public class EditorDrawer_Vector2 : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label.text = $"(Fix64 Vector2) {property.name}";
            FVector2 f2 = (FVector2)property.boxedValue;
            Vector2 uv2 = EditorGUI.Vector2Field(position, label,
                new Vector2(
                    (float)f2.X,
                    (float)f2.Y)
            );
            FVector2 newF2 = new FVector2(
                (Fix64)uv2.x,
                (Fix64)uv2.y
            );
            property.boxedValue = (newF2) as object;
        }
    }
}
#endif