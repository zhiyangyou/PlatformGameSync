using System.Collections;
using System.Collections.Generic;
using FixMath.NET;
using UnityEngine;

public class testFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        Fix64 f1 = (Fix64)0.5f;
        var f = (float)f1;
        Debug.LogError(f);
        Fix64 Deg2Rad = (Fix64)(0.017453292f);
        Fix64 Rad2Deg = (Fix64)(57.29578f);
        int a = 0;
    }
}
