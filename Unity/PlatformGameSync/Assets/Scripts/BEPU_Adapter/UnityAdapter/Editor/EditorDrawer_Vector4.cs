#if UNITY_EDITOR
using FixMath.NET;
using UnityEditor;
using UnityEngine;
using FVector4 = BEPUutilities.Vector4;


namespace BEPU_Adapter.UnityAdapter {
    [CustomPropertyDrawer(typeof(FVector4))]
    public class EditorDrawer_Vector4 : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label.text = $"(Fix64 Vector4) {property.name}";
            FVector4 f4 = (FVector4)property.boxedValue;
            Vector4 uv4 = EditorGUI.Vector4Field(position, label,
                new Vector4(
                    (float)f4.X,
                    (float)f4.Y,
                    (float)f4.Z,
                    (float)f4.W
                )
            );
            FVector4 newF4 = new FVector4(
                (Fix64)uv4.x,
                (Fix64)uv4.y,
                (Fix64)uv4.z,
                (Fix64)uv4.w
            );

            property.boxedValue = (newF4) as object;
        }
    }
}
#endif