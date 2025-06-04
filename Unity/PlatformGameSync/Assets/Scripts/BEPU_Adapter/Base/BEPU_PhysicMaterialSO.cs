using System;
using System.Collections;
using System.Collections.Generic;
using FixMath.NET;
using UnityEngine;


[CreateAssetMenu(fileName = "BEPU_PhysicsMaterial", menuName = "BEPU/BEPU_PhysicsMaterial", order = 0)]
public class BEPU_PhysicMaterialSO : ScriptableObject {
    [SerializeField] public BEPU_PhysicMaterial Data;
    public Fix64 FBounciness => Data.FBounciness;
    public Fix64 FKineticFriction => Data.FKineticFriction;
    public Fix64 FStaticFriction => Data.FStaticFriction;
}

[Serializable]
public class BEPU_PhysicMaterial {
    [SerializeField] public float Bounciness;
    [SerializeField] public float KineticFriction;
    [SerializeField] public float StaticFriction;

    public Fix64 FBounciness => (Fix64)Bounciness;
    public Fix64 FKineticFriction => (Fix64)KineticFriction;
    public Fix64 FStaticFriction => (Fix64)StaticFriction;

    public void SyncToBEPUMat(BEPUphysics.Materials.Material mat) {
        mat.Bounciness = this.FBounciness;
        mat.KineticFriction = this.FKineticFriction;
        mat.StaticFriction = this.FStaticFriction;
    }
}