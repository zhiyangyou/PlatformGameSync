#if UNITY_EDITOR
using FixMath.NET;
using UnityEditor;
using UnityEngine;
using FVector3 = BEPUutilities.Vector3;


namespace BEPU_Adapter.UnityAdapter {
    [CustomPropertyDrawer(typeof(FVector3))]
    public class EditorDrawer_Vector3 : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label.text = $"(Fix64 Vector3) {property.name}";
            FVector3 f3 = (FVector3)property.boxedValue;
            Vector3 uv3 = EditorGUI.Vector3Field(position, label,
                new Vector3(
                    (float)f3.X,
                    (float)f3.Y,
                    (float)f3.Z
                )
            );
            FVector3 newF3 = new FVector3(
                (Fix64)uv3.x,
                (Fix64)uv3.y,
                (Fix64)uv3.z
            );
            property.boxedValue = newF3;
        }
    }
}
#endif