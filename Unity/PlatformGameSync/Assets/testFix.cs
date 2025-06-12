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
        Fix64 f981 = (Fix64)(9.81f);
        Fix64 minusf981 = (Fix64)(-9.81f);
    }
}
