using BEPUphysics.Entities;
using UnityEngine;

public abstract class BEPU_BaseCollider : MonoBehaviour {
    public abstract BEPU_CustomEntity entity { get; }

    public abstract BEPU_EEntityType BEPUEntityType { get; }
}