using System.Collections;
using System.Collections.Generic;
using FixMath.NET;
using NUnit.Framework;
using UnityEngine;




public class TestBUPEFix64  
{

  
  
  [Test]
  public void Fix64_Float() {
    float f = 3.14f;
    Fix64 f64 = (Fix64)f;

     var realF = (float)f64;
    Debug.Log($"f:{f} realF:{realF} f64:{f64} ");
    Assert.IsTrue(Mathf.Approximately(f, realF));

  }
  
  [Test]
  public void Fix64_Double() {
      double f = 3.14;
      Fix64 f64 = (Fix64)f;

      var realF = (double)f64;
      Debug.Log($"f:{f} realF:{realF} f64:{f64} ");
      Assert.IsTrue((realF-f)<0.001);
  }
  
  [Test]
  public void Fix64_int() {
      int f = 101;
      Fix64 f64 = (Fix64)f;

      var realF = (int)f64;
      Debug.Log($"f:{f} realF:{realF} f64:{f64} ");
      Assert.IsTrue((realF-f)==0);
  }
}
