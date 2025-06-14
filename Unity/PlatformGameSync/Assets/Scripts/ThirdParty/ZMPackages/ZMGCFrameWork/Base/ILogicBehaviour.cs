using System.Collections;
using System.Collections.Generic;
using FixMath.NET;
using UnityEngine;

public interface ILogicBehaviour  
{
    void OnCreate();

    void OnLogicFrameUpdate(Fix64 deltaTime);
    
    void OnDestroy();
}
