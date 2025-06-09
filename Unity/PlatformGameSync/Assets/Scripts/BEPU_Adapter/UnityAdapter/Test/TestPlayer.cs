using System;
using System.Collections;
using BEPU_Adapter.Pool;
using BEPUutilities;
using FixMath.NET;
using UnityEngine;
using BoundingSphere = BEPUutilities.BoundingSphere;
using Vector3 = UnityEngine.Vector3;


public class TestPlayer : MonoBehaviour {
    [NonSerialized] private BEPU_BaseColliderMono colliderMono;
    [NonSerialized] private Animator _animator;
    [NonSerialized] private SpriteRenderer _spRender;

    public Transform trAttackPoint;
    public float moveSpeed = 3f;
    public float jumpForce = 8f;
    public bool _facingRight = true;
    public bool isRectHitCheck = true;

    public float groundCheckDictance = 0.5f;

    public bool isGround = false;

    private float _xInput = 0f;

    private BoundingBox? _attackBoundingBox = null;
    private BoundingSphere? _attackBoundingSphere = null;


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
            BEPU_LayerDefine.Envirement,
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
            var results = ListPool<BEPU_BaseColliderLogic>.Get();
            if (isRectHitCheck) {
                var center = trAttackPoint.position.ToFixedVector3();
                var halfSize = BEPUutilities.Vector3.One * (Fix64)0.5f;
                _attackBoundingBox = new BoundingBox(center - halfSize, center + halfSize);
                BEPU_PhysicsManagerUnity.Instance.OverlapBoxAll(center, halfSize, BEPU_LayerDefine.Envirement, results);
            }
            else {
                var center = trAttackPoint.position.ToFixedVector3();
                Fix64 radiu = (Fix64)0.5f;
                _attackBoundingSphere = new BoundingSphere(center, radiu);
                BEPU_PhysicsManagerUnity.Instance.OverlapCircleAll(center, radiu, BEPU_LayerDefine.Envirement, results);
            }

            foreach (var logic in results) {
                // Debug.LogError($"hitObj: {(logic.RenderObj as BEPU_BaseColliderMono).gameObject.name}");
            }
            ListPool<BEPU_BaseColliderLogic>.Release(results);
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

        if (_attackBoundingBox.HasValue) {
            var box = _attackBoundingBox.Value;
            Gizmos.DrawWireCube(box.Center.ToUnityVector3(), box.Size.ToUnityVector3());
            StartCoroutine(IEHideHitGizmos());
        }
        if (_attackBoundingSphere.HasValue) {
            Gizmos.DrawSphere(_attackBoundingSphere.Value.Center.ToUnityVector3(), (float)_attackBoundingSphere.Value.Radius);
            StartCoroutine(IEHideHitGizmos());
        }
    }

    IEnumerator IEHideHitGizmos() {
        for (int i = 0; i < 200; i++) {
            yield return new WaitForEndOfFrame();
        }
        _attackBoundingBox = null;
        _attackBoundingSphere = null;
    }
}