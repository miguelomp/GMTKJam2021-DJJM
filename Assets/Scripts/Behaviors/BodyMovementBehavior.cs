using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BodyMovementBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 3f;
    public float jumpForce = 1f;

    private Vector3 movementDirection;
    private bool grounded = false;

    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public bool IsGrounded() { return grounded; }

    public void UpdateMovementData(Vector3 newMovementDirection)
    {
        movementDirection = newMovementDirection;
    }

    public void Jump()
    {
        if (grounded)
        {
            grounded = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        Vector3 movement = CameraDirection(movementDirection) * movementSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);
    }

    private void OnCollisionStay(Collision collision)
    {
        grounded = true;
    }

    Vector3 CameraDirection(Vector3 movementDirection)
    {
        var cameraForward = Camera.main.transform.forward;
        var cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        return cameraForward * movementDirection.z + cameraRight * movementDirection.x;

    }
}
