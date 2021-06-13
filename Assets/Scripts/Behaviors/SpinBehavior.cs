using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpinBehavior : MonoBehaviour
{
    HeadBehavior headBehavior;

    HingeJoint hj;
    Rigidbody rb;
    
    public Transform headFixedPos;
    public Rigidbody headPivotPos;

    private void Start()
    {
        headBehavior = gameObject.GetComponent<HeadBehavior>();

        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;

        transform.position = headFixedPos.position;
    }

    public void StartMotor()
    {
        JointMotor motor = hj.motor;

        motor.force = 10f;
        motor.targetVelocity = 1000f;
        hj.motor = motor;

        hj.useMotor = true;
    }

    public void Detach()
    {
        if (hj != null)
        {
            hj.breakForce = 0;
            //rb.isKinematic = false;
            //StartCoroutine(StartDetach());
        }

    }

    IEnumerator StartDetach()
    {
        while (true)
        {
            Debug.Log(hj.angle);
            if (hj.axis == Vector3.up && hj.angle > 45f - 1 && hj.angle < 45f + 1)
            {
                Debug.Log("LAUNCHH!!!!!!!!!!!!!!!!!!!! RIGHT");
                break;
            }
            if (hj.axis == Vector3.down && hj.angle > - 45f - 1 && hj.angle < - 45f + 1)
            {
                Debug.Log("LAUNCHH!!!!!!!!!!!!!!!!!!!! LEFT");
                break;
            }

            yield return new WaitForEndOfFrame();
        }
        hj.breakForce = 0;
        yield return null;
    }

    public void ClockWise()
    {
        Spin(Vector3.up);
    }

    public void AntiClockWise()
    {
        Spin(Vector3.down);
    }

    void Spin(Vector3 dir)
    {
        if (hj == null)
        {
            hj = gameObject.AddComponent<HingeJoint>();

            //rb.isKinematic = true;
            rb.useGravity = true;

            hj.connectedBody = headPivotPos;
            hj.axis = dir;
            hj.autoConfigureConnectedAnchor = false;
            hj.connectedAnchor = Vector3.zero;
            hj.anchor = new Vector3(0, 0, -.09f);
        }
    }

    
}
