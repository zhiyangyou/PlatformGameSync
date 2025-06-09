using BEPU_Adapter.Pool;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUutilities;
using FixMath.NET;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;


public class TestPlayer : MonoBehaviour {
    [FormerlySerializedAs("_collider")] public BEPU_BaseColliderMono colliderMono;
    public Animator _animator;
    public SpriteRenderer _spRender;
    public float moveSpeed = 3f;
    public float jumpForce = 8f;
    public bool _facingRight = true;

    public float groundCheckDictance = 0.5f;

    public LayerMask whatIsGround;

    public bool isGround = false;

    private float _xInput = 0f;

    private void Awake() {
        colliderMono = GetComponent<BEPU_BaseColliderMono>();
        _animator = GetComponent<Animator>();
        _spRender = GetComponent<SpriteRenderer>();
    }

    private void HanldeAnimations() {
        _animator.SetBool("isGrounded", isGround);
        _animator.SetFloat("xVelocity", (float)colliderMono.entity.LinearVelocity.X);
        _animator.SetFloat("yVelocity", (float)colliderMono.entity.LinearVelocity.Y);
    }

    private void HandleCollision() {
        var listRet = ListPool<BEPU_RayCastReuslt>.Get();


        BEPU_PhysicsManagerUnity.Instance.Raycast(
            transform.position.ToFixedVector3(),
            BEPUutilities.Vector3.Down,
            (Fix64)groundCheckDictance,
            BEPU_LayerDefaine.Envirement,
            listRet
        );
        isGround = false;
        if (listRet.Count > 0) {
            foreach (var reuslt in listRet) {
                if (reuslt.baseLogic.RenderObj is BEPU_BoxColliderMono mono) {
                    if (mono.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                        isGround = true;
                        break;
                    }
                }
            }
        }
        ListPool<BEPU_RayCastReuslt>.Release(listRet);
    }

    private void HandleMove() {
        var oldV = colliderMono.entity.LinearVelocity;
        oldV.X = (Fix64)_xInput * (Fix64)moveSpeed;
        colliderMono.entity.LinearVelocity = oldV;
    }

    private void HandleInput() {
        _xInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space)) {
            TryJump();
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            TryAttack();
        }
    }

    private void HandleFlip() {
        Fix64 threshold = (Fix64)0.01f;
        var hasV = Fix64.Abs(colliderMono.entity.LinearVelocity.X) > threshold;
        var isFlip = hasV && colliderMono.entity.LinearVelocity.X < threshold;
        if (hasV && _facingRight != isFlip) {
            _facingRight = isFlip;
        }
        _spRender.flipX = _facingRight;
    }

    private void TryJump() {
        if (isGround) {
            var oldV = colliderMono.entity.LinearVelocity;
            oldV.Y = (Fix64)jumpForce;
            colliderMono.entity.LinearVelocity = oldV;
        }
    }

    private void TryAttack() {
        if (isGround) {
            _animator.SetTrigger("attack");
        }
    }

    private void Update() {
        HandleMove();
        HandleInput();
        HanldeAnimations();
        HandleFlip();
        HandleCollision();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, transform.position + groundCheckDictance * Vector3.down);
    }
}