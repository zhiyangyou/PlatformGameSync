using System;
using FixMath.NET;
using UnityEngine;
using Vector3 = BEPUutilities.Vector3;


public class TestPlayer : MonoBehaviour {
    public BEPU_BaseCollider _collider;
    public  Animator _animator;
    public SpriteRenderer _spRender;
    public float moveSpeed = 3f;
    public float jumpForce = 8f;
    public bool _facingRight = true;

    private float _xInput = 0f;
    private bool _isMoving = false;

    private void Awake() {
        _collider = GetComponent<BEPU_BaseCollider>();
        _animator = GetComponent<Animator>();
        _spRender = GetComponent<SpriteRenderer>();
    }

    private void HanldeAnimations() {
        _isMoving = _collider.entity.LinearVelocity.X != Fix64.Zero;
        _animator.SetBool("IsMoving", _isMoving);
    }

    private void HandleMove() {
        var oldV = _collider.entity.LinearVelocity;
        oldV.X = (Fix64)_xInput * (Fix64)moveSpeed;
        _collider.entity.LinearVelocity = oldV;
    }

    private void HandleJump() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            var oldV = _collider.entity.LinearVelocity;
            oldV.Y = (Fix64)jumpForce;
            _collider.entity.LinearVelocity = oldV;
        }
    }

    private void HandleFlip() {
        var isFlip = _collider.entity.LinearVelocity.X < Fix64.Zero;
        _facingRight = isFlip;
        _spRender.flipX = _facingRight;
    }
    private void Update() {
        _xInput = Input.GetAxis("Horizontal");
        HandleMove();
        HandleJump();
        HanldeAnimations();
        HandleFlip();
    }
}