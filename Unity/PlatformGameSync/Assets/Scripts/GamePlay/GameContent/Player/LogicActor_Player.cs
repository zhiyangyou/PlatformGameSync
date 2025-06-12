using BEPUphysics.CollisionShapes.ConvexShapes;
using WorldSpace.GameWorld;

public partial class LogicActor_Player {
    RenderObject_Player _renderPlayer => RenderObject as RenderObject_Player;
    public bool IsLocalActor { get; private set; }

    public InputSystem_Player InputSystem { get; private set; }

    private NextFrameTimer _nextFrameTimer;

    public BEPU_CustomEntity PhysicsEntity { get; private set; } = null;
    public BoxShape BoxShape { get; private set; } = null;

    private void InitPlayeColliderAttrs() { }


    public override void OnCreate() {
        this.ColliderType = BEPU_ColliderType.Box;
        base.OnCreate();
        _nextFrameTimer = WorldManager.GetWorld<GameWorld>().nextFrameTimer;
        PhysicsEntity = BaseColliderLogic.entity;
        BoxShape = BaseColliderLogic.entityShape as BoxShape;
        InitPlayeColliderAttrs();
        InitInputSystem();
        InitStateMachine();
    }

    public override void OnLogicFrameUpdate() {
        base.OnLogicFrameUpdate();
        LogicFrameUpdate_StateMachine();
    }

    public override void OnDestory() {
        DisposeInputActions();
        InputSystem.Disable();
        _nextFrameTimer = null;
        base.OnDestory();
    }

    public void SetIsLocalPlayer(bool v) {
        IsLocalActor = v;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="facingRight"></param>
    public void SetY180(bool facingRight) {
        var q = this.BaseColliderLogic.entity.Orientation;
        var e = q.ToEulerAngles();
        e.Y = facingRight ? 0 : -180;
        var newQ = e.ToQuaternion();
        this.BaseColliderLogic.entity.Orientation = newQ;
    }
}