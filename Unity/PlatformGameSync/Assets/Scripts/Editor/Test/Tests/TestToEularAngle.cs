using System;
using FixMath.NET;
using NUnit.Framework;
using UnityEngine;

public class TestToEularAngle {
    public Vector3 AddCycles(Vector3 v, int cycle = 10) {
        return new Vector3(v.x + cycle * 360, v.x + cycle * 360, v.x + cycle * 360);
    }

    public bool AreSameEularAngle(Vector3 v1, Vector3 v2, float wuca = 0.1f) {
        v1 = AddCycles(v1);
        v2 = AddCycles(v2);

        v1.x = v1.x % 360;
        v1.y = v1.y % 360;
        v1.z = v1.z % 360;

        v2.x = v2.x % 360;
        v2.y = v2.y % 360;
        v2.z = v2.z % 360;

        return Mathf.Abs(v1.x - v2.x) < wuca
               && Mathf.Abs(v1.y - v2.y) < wuca
               && Mathf.Abs(v1.z - v2.z) < wuca
            ;
    }

    int step = 15;
    float 误差 = 0.05f;
    private int start = -720;
    private int end = 720;

    [Test]
    public void QToEular() {
        for (int x = start; x <= end; x += step) {
            for (int y = start; y <= end; y += step) {
                for (int z = start; z <= end; z += step) {
                    var rotV3 = new Vector3(x, y, z);
                    var unityQ = Quaternion.Euler(rotV3);

                    var FQ = new BEPUutilities.Quaternion((Fix64)unityQ.x, (Fix64)unityQ.y, (Fix64)unityQ.z, (Fix64)unityQ.w);
                    var UQ = new Quaternion(unityQ.x, unityQ.y, unityQ.z, unityQ.w);
                    var fv3 = FQ.ToEulerAngles();
                    var uv3 = UQ.eulerAngles;

                    var areEqual = AreSameEularAngle(uv3, fv3.ToUnityVector3());
                    Assert.IsTrue(areEqual);
                }
            }
        }
    }

    [Test]
    public void EularToQ() {
    
        for (int x = start; x <= end; x += step) {
            for (int y = start; y <= end; y += step) {
                for (int z = start; z <= end; z += step) {
                    var rotV3 = new Vector3(x, y, z);
                    {
                        var newUQ = Quaternion.Euler(rotV3);
                        var newFQ = rotV3.ToFixedVector3().ToQuaternion();
                        
                        Assert.IsTrue(Mathf.Abs(((float)newFQ.X - newUQ.x)) < 误差);
                        Assert.IsTrue(Mathf.Abs(((float)newFQ.Y - newUQ.y)) < 误差);
                        Assert.IsTrue(Mathf.Abs(((float)newFQ.Z - newUQ.z)) < 误差);
                        Assert.IsTrue(Mathf.Abs(((float)newFQ.W - newUQ.w)) < 误差);
                    }
                }
            }
        }
    }
}