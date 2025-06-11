using FixMath.NET;
using NUnit.Framework;
using UnityEngine;

public class TestASin {
    [Test]
    public void TestFix64ASin() {
        for (int i = 0; i < 100000; i++) {
            var drgee = i;
            var rad = drgee / 180.0f * Mathf.PI;
            var sinV = Mathf.Sin(rad);

            var ret1 = Mathf.Asin(sinV);

            var ret2 = Fix64.ASin((Fix64)sinV);
            // Debug.Log($"{ret1:F3} {(float)ret2:F3}");
            Assert.IsTrue((ret1 - (float)ret2) < 0.0001f);
        }
    }
}