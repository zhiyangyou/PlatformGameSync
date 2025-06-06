using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Entities;
using BEPUutilities;
using FixMath.NET;

public static class EntityEx {
    public static Matrix3x3 AutoLocalInertiaTensor(this Entity entity, Fix64 mass) {
        return entity.CollisionInformation.Shape.VolumeDistribution * (InertiaHelper.InertiaTensorScale * mass);
    }
}