using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PivotTorque : MonoBehaviour
{
    
    public Rigidbody rb;
    public Vector3 torqueDirection = Vector3.right;
    public float torqueMagnitude = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(torqueDirection, Time.deltaTime * torqueMagnitude);
    }

    private void FixedUpdate()
    {
        //rb.AddTorque(torqueDirection * torqueMagnitude);
    }
}
