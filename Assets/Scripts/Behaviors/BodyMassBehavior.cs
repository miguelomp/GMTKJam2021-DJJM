using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BodyMassBehavior : MonoBehaviour
{
    public Rigidbody head;
    public Transform headFixed;

    private Rigidbody rb;
    private GameObject backup;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        backup = new GameObject();
    }

    private void Update()
    {
        if (backup != null)
        {
            backup.transform.position = transform.position;
        }
    }

    public void UnsetBody()
    {
        // erase constrains
        rb.constraints = RigidbodyConstraints.None;
    }

    public void SetBody()
    {
        // recover the position with the copy backup position
        // make constrains
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        transform.position = backup.transform.position + Vector3.up * 2;
        transform.rotation = Quaternion.identity;

        rb.isKinematic = false;

        //head.isKinematic = true;
        head.useGravity = false;

        head.transform.position = headFixed.position;
        head.transform.rotation = headFixed.rotation;
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }
}
