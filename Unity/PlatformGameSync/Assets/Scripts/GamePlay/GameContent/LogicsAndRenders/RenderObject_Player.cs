using BEPUphysics.CollisionShapes.ConvexShapes;
using FixMath.NET;
using UnityEngine;
using FVector3 = BEPUutilities.Vector3;

public class RenderObject_Player : RenderObject {
    [SerializeField] private float capsuleLen = 1;
    [SerializeField] private float capsuleRadiu = 0.5f;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeedAirRate = 0.7f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundRayCastLen = 1.3f;
    [SerializeField] private float wallRayCastLen = 1.3f;
    [SerializeField] private BEPU_LayerDefine whatIsGround = BEPU_LayerDefine.Envirement;
    public Animator Animator => animator;

    private LogicActor_Player _logicPlayer => this.LogicObject as LogicActor_Player;

    public override void SyncExtendAttrsToEntity() {
        if (base.baseColliderType != null) {
            BEPU_CapsuleColliderMono.SyncExtendAttrsToEntity(baseColliderLogic as BEPU_CapsuleColliderLogic, (Fix64)capsuleRadiu, (Fix64)capsuleLen);
        }
    }

    private void SyncSerialfield2ToLogic() {
        if (_logicPlayer != null) {
            _logicPlayer.moveSpeedAirRate = (Fix64)moveSpeedAirRate;
            _logicPlayer.moveSpeed = (Fix64)moveSpeed;
            _logicPlayer.jumpForce = (Fix64)jumpForce;
            _logicPlayer.groundRayCastLen = (Fix64)groundRayCastLen;
            _logicPlayer.wallRayCastLen = (Fix64)wallRayCastLen;
            _logicPlayer.whatIsGround = whatIsGround;
            _logicPlayer.capsuleLen = (Fix64)capsuleLen;
            _logicPlayer.capsuleRadiu = (Fix64)capsuleRadiu;
        }
    }

    public override void OnCreate() {
        base.OnCreate();
        SyncSerialfield2ToLogic();
    }
#if UNITY_EDITOR
    protected virtual void OnValidate() {
        base.OnValidate();
        SyncSerialfield2ToLogic();
    }


    private void DrawWallDetectGizmos() {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.yellow;
        var len = Application.isPlaying ? (float)_logicPlayer.wallRayCastLen : this.wallRayCastLen;
        Fix64 facingDir = Application.isPlaying ? _logicPlayer.facingDir : Fix64.One;
        var p1 = (baseColliderLogic.entity.Position).ToUnityVector3();
        Gizmos.DrawLine(p1, p1 + len * Vector3.right * (float)facingDir);
        Gizmos.color = oldColor;
    }

    private void DrawGroundDetectGizmos() {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.red;
        var len = Application.isPlaying ? (float)_logicPlayer.groundRayCastLen : this.groundRayCastLen;
        var p1 = (baseColliderLogic.entity.Position).ToUnityVector3();
        Gizmos.DrawLine(p1, p1 + len * Vector3.down);
        Gizmos.color = oldColor;
    }

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();
        DrawGroundDetectGizmos();
        DrawWallDetectGizmos();
    }
#endif
}