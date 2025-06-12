using System;
using FixMath.NET;
using UnityEngine;


[CreateAssetMenu(fileName = "BEPU_PhysicsMaterial", menuName = "BEPU/BEPU_PhysicsMaterial", order = 0)]
public class BEPU_PhysicMaterialSO : ScriptableObject {
    [SerializeField] public BEPU_PhysicMaterial Data;
    public Fix64 Bounciness => Data.Bounciness;
    public Fix64 KineticFriction => Data.KineticFriction;
    public Fix64 StaticFriction => Data.StaticFriction;
}