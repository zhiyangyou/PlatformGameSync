using FixMath.NET;
using UnityEngine;
using FVector3 = BEPUutilities.Vector3;

public class RenderObject_Player : RenderObject {
    [SerializeField] private Animator animator;

    [SerializeField] private Fix64 capsuleLen = Fix64.One;
    [SerializeField] private Fix64 capsuleRadiu = Fix64.HalfOne;
    [SerializeField] private Fix64 moveSpeedAirRate;
    [SerializeField] private Fix64 moveSpeed;
    [SerializeField] private Fix64 jumpForce;
    [SerializeField] private Fix64 groundRayCastLen;
    [SerializeField] private Fix64 wallRayCastLen;
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
            _logicPlayer.moveSpeedAirRate = moveSpeedAirRate;
            _logicPlayer.moveSpeed = moveSpeed;
            _logicPlayer.jumpForce = jumpForce;
            _logicPlayer.groundRayCastLen = groundRayCastLen;
            _logicPlayer.wallRayCastLen = wallRayCastLen;
            _logicPlayer.capsuleRadiu = capsuleRadiu;
            _logicPlayer.capsuleLen = capsuleLen;
            _logicPlayer.whatIsGround = whatIsGround;
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
        Fix64 len = Application.isPlaying ? _logicPlayer.wallRayCastLen : this.wallRayCastLen;
        Fix64 facingDir = Application.isPlaying ? _logicPlayer.facingDir : Fix64.One;
        FVector3 p1 = baseColliderLogic.entity.Position;
        Gizmos.DrawLine(p1.ToUnityVector3(), (p1 + len * FVector3.Right * facingDir).ToUnityVector3());
        Gizmos.color = oldColor;
    }

    private void DrawGroundDetectGizmos() {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.red;
        Fix64 len = Application.isPlaying ? _logicPlayer.groundRayCastLen : this.groundRayCastLen;
        FVector3 p1 = baseColliderLogic.entity.Position;
        Gizmos.DrawLine(p1.ToUnityVector3(), (p1 + len * FVector3.Down).ToUnityVector3());
        Gizmos.color = oldColor;
    }

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();
        DrawGroundDetectGizmos();
        DrawWallDetectGizmos();
    }
#endif
}