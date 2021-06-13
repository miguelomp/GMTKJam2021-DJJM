using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HeadBehavior : MonoBehaviour
{
    [Header("Properties")]
    public float breakForceMultiplier = .1f;
    public float headMovementMultiplier = 1f;

    [Header("Sub Behaviours")]
    public BodyMassBehavior bodyMassBehavior;

    private Rigidbody rb;
    private float torqueSpeed = 0f;
    private Vector3 movementDirection;

    private bool isHeadOff = false;

    FixedJoint neck;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        neck = gameObject.AddComponent<FixedJoint>();
        neck.connectedBody = bodyMassBehavior.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (isHeadOff)
        {
            //rb.AddForce(new Vector3(0f, .15f, 1) * torqueSpeed * headMovementMultiplier);
            MoveThePlayer();
        }
        
    }

    public void HeadOff()
    {
        if (neck != null)
        {
            Destroy(neck);
        }
        isHeadOff = true;
    }

    public void HeadOn()
    {
        isHeadOff = false;

        if (neck == null)
        {
            neck = gameObject.AddComponent<FixedJoint>();
            neck.connectedBody = bodyMassBehavior.GetComponent<Rigidbody>();
        }
    }

    public void UpdateMovementData(float magnitude, Vector3 movement)
    {
        torqueSpeed = movement.magnitude;
        movementDirection = movement;
    }

    private void OnJointBreak(float breakForce)
    {
        Debug.Log(breakForce);
        breakForce = Mathf.Clamp(breakForce, 0f, 20f);
        JoinBreak(breakForce);
    }

    public void JoinBreak(float breakForce)
    {
        bodyMassBehavior.UnsetBody();
        rb.AddForce((rb.velocity + bodyMassBehavior.GetVelocity()) * breakForce * breakForceMultiplier, ForceMode.Impulse);
    }

    public void JoinBreak(Vector3 dir, float breakForce)
    {
        bodyMassBehavior.UnsetBody();
        rb.AddForce(dir * breakForce * .1f, ForceMode.Impulse);
    }

    void MoveThePlayer()
    {
        Vector3 movement = CameraDirection(movementDirection) * headMovementMultiplier * Time.fixedDeltaTime;
        //rb.AddForce(movement.normalized * headMovementMultiplier, ForceMode.Impulse);
        rb.MovePosition(transform.position + movement);
        //rb.AddForce(movement * headMovementMultiplier);
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
