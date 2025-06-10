using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : World {
    #region override

    private static Type[] s_OrderCtrlTypes = new Type[] { };

    private static Type[] s_OrderDataTypes = new Type[]
        { };

    private static Type[] s_OrderMsgTypes = new Type[]
        { };


    public override Type[] GetLogicBehaviourExecution() {
        return s_OrderCtrlTypes;
    }

    public override Type[] GetDataBehaviourExecution() {
        return s_OrderDataTypes;
    }

    public override Type[] GetMsgBehaviourExecution() {
        return s_OrderMsgTypes;
    }

    public override WorldEnum WorldEnum => WorldEnum.GameWorld;

    #endregion
    
    
    
}