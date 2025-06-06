using System;
using FixMath.NET;
using UnityEngine;


[CreateAssetMenu(fileName = "BEPU_PhysicsMaterial", menuName = "BEPU/BEPU_PhysicsMaterial", order = 0)]
public class BEPU_PhysicMaterialSO : ScriptableObject {
    [SerializeField] public BEPU_PhysicMaterial Data;
    public Fix64 FBounciness => Data.FBounciness;
    public Fix64 FKineticFriction => Data.FKineticFriction;
    public Fix64 FStaticFriction => Data.FStaticFriction;
}