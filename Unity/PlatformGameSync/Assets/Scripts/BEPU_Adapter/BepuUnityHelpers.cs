// using UnityEngine;
// using BEPUutilities; // For FixedV3, FixedQuaternion, Fixed64
//
// public static class BepuUnityHelpers
// {
//     // BEPUphysics v1 (Fixed64) uses a specific internal representation.
//     // Fixed64.ToFloat() and Fixed64.FromFloat() are the correct conversion methods.
//
//     public static Vector3 ToUnityVector(FixedV3 bepuVector)
//     {
//         return new Vector3(
//             Fixed64.ToFloat(bepuVector.X),
//             Fixed64.ToFloat(bepuVector.Y),
//             Fixed64.ToFloat(bepuVector.Z)
//         );
//     }
//
//     public static FixedV3 ToBepuVector(Vector3 unityVector)
//     {
//         return new FixedV3(
//             Fixed64.FromFloat(unityVector.x),
//             Fixed64.FromFloat(unityVector.y),
//             Fixed64.FromFloat(unityVector.z)
//         );
//     }
//
//     public static Quaternion ToUnityQuaternion(FixedQuaternion bepuQuaternion)
//     {
//         return new Quaternion(
//             Fixed64.ToFloat(bepuQuaternion.X),
//             Fixed64.ToFloat(bepuQuaternion.Y),
//             Fixed64.ToFloat(bepuQuaternion.Z),
//             Fixed64.ToFloat(bepuQuaternion.W)
//         );
//     }
//
//     public static FixedQuaternion ToBepuQuaternion(Quaternion unityQuaternion)
//     {
//         return new FixedQuaternion(
//             Fixed64.FromFloat(unityQuaternion.x),
//             Fixed64.FromFloat(unityQuaternion.y),
//             Fixed64.FromFloat(unityQuaternion.z),
//             Fixed64.FromFloat(unityQuaternion.w)
//         );
//     }
// }