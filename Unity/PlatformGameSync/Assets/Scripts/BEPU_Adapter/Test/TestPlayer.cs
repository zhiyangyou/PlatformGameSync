using System;
using FixMath.NET;
using UnityEngine;
using Vector3 = BEPUutilities.Vector3;


public class TestPlayer : MonoBehaviour {
    public BEPU_BaseCollider sphereCollider;

    public float moveSpeed = 3f;
    public float jumpForce = 8f;

    private float xInput = 0f;

    private void Awake() { }

    private void Update() {
        xInput = Input.GetAxis("Horizontal");

        // 水平移动
        {
            var oldV = sphereCollider.entity.LinearVelocity;
            oldV.X = (Fix64)xInput * (Fix64)moveSpeed;
            sphereCollider.entity.LinearVelocity = oldV;
        }

        // 跳
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                var oldV = sphereCollider.entity.LinearVelocity;
                oldV.Y = (Fix64)jumpForce;
                sphereCollider.entity.LinearVelocity = oldV;
            }
        }
    }
}