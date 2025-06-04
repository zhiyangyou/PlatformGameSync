using NUnit.Framework;
using FixMath.NET;
using UnityEditor.VersionControl;
using UnityEngine;
using UVector3 = UnityEngine.Vector3;
using FVector3 = BEPUutilities.Vector3;
using UVector2 = UnityEngine.Vector2;
using FVector2 = BEPUutilities.Vector2;
using UQuaternion = UnityEngine.Quaternion;
using FQuaternion = BEPUutilities.Quaternion;

public class TestFixedConverter {
    [Test]
    public void TestV3_1() {
        UVector3 unityV3 = new UVector3(1.11f, 2.22f, 3.33f);
        var fv3 = unityV3.ToFixedVector3();
        // Debug.Log($"{fv3.X} {fv3.Y} {fv3.Z}");


        Assert.IsTrue(Mathf.Approximately(unityV3.x, (float)fv3.X));
        Assert.IsTrue(Mathf.Approximately(unityV3.y, (float)fv3.Y));
        Assert.IsTrue(Mathf.Approximately(unityV3.z, (float)fv3.Z));
    }


    [Test]
    public void TestV3_2() {
        FVector3 fv3 = new FVector3((Fix64)1.11f, (Fix64)2.22f, (Fix64)3.33f);
        UVector3 uv3 = fv3.ToUnityVector3();
        Assert.IsTrue(Mathf.Approximately(uv3.x, (float)fv3.X));
        Assert.IsTrue(Mathf.Approximately(uv3.y, (float)fv3.Y));
        Assert.IsTrue(Mathf.Approximately(uv3.z, (float)fv3.Z));
    }


    [Test]
    public void TestV2_1() {
        UVector2 unityV2 = new UVector2(1.11f, 2.22f);
        var fv2 = unityV2.ToFixedVector2();
        // Debug.Log($"{fv2.X} {fv2.Y} ");

        Assert.IsTrue(Mathf.Approximately(unityV2.x, (float)fv2.X));
        Assert.IsTrue(Mathf.Approximately(unityV2.y, (float)fv2.Y));
    }


    [Test]
    public void TestV2_2() {
        FVector2 fv2 = new FVector2((Fix64)1.11f, (Fix64)2.22f);
        UVector2 uv2 = fv2.ToUnityVector2();
        Assert.IsTrue(Mathf.Approximately(uv2.x, (float)fv2.X));
        Assert.IsTrue(Mathf.Approximately(uv2.y, (float)fv2.Y));
    }

    [Test]
    public void TestQ_1() {
        var uq = new UQuaternion(1.11f, 2.22f, 3.33f, 4.44f);
        var fq = uq.ToFixedQuaternion();
        Assert.IsTrue(Mathf.Approximately(uq.x, (float)fq.X));
        Assert.IsTrue(Mathf.Approximately(uq.y, (float)fq.Y));
        Assert.IsTrue(Mathf.Approximately(uq.z, (float)fq.Z));
        Assert.IsTrue(Mathf.Approximately(uq.w, (float)fq.W));
    }
    
    
    [Test]
    public void TestQ_2() {
        var fq = new FQuaternion((Fix64)1.11f, (Fix64)2.22f, (Fix64)3.33f, (Fix64)4.44f);
        var uq = fq.ToUnityQuaternion();
        Assert.IsTrue(Mathf.Approximately(uq.x, (float)fq.X));
        Assert.IsTrue(Mathf.Approximately(uq.y, (float)fq.Y));
        Assert.IsTrue(Mathf.Approximately(uq.z, (float)fq.Z));
        Assert.IsTrue(Mathf.Approximately(uq.w, (float)fq.W));
    }
}