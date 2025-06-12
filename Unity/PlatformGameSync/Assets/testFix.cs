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
        var f =(Fix64)0.999999f;
        Debug.LogError(f.RawValue);
        Debug.LogError($"{(float)Fix64.ZeroPoint999999}");
    }
}
