using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeBehavior : MonoBehaviour
{
    //Objects that will interact with the rope
    public Transform whatTheRopeIsConnectedTo;
    public Transform whatIsHangingFromTheRope;

    [Header("Rope properties")]
    public float ropeIncrement = 5f;

    //Line renderer used to display the rope
    private LineRenderer lineRenderer;
    private bool showRope = false;

    //Rope data
    [SerializeField]
    private float ropeLength;
    private float minRopeLength = .03f;
    private float maxRopeLength = 100f;
    //Mass of what the rope is carrying
    private float loadMass = 100f;
    //How fast we can add more/less rope
    float winchSpeed = 5f;
    bool canGet = false;
    bool canLet = false;

    //The joint we use to approximate the rope
    SpringJoint springJoint;

    //A list with all rope sections
    public List<Vector3> allRopeSections = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {

        ropeLength = ropeIncrement;
        maxRopeLength = ropeIncrement;

        whatIsHangingFromTheRope = transform.parent;
        whatTheRopeIsConnectedTo = transform;

        //Init the line renderer we use to display the rope
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Add more/less rope
        //UpdateWinch();

        //Display the rope with a line renderer
        if (showRope)
        {
            DisplayRope();
        }

        if (canGet)
        {
            GettingRope();
        }
    }

    private void OnJointBreak(float breakForce)
    {
        // add impulse if necesary
        //this happens when hinged joint breaks
    }

    public void CreateRope()
    {
        // create rope
        springJoint = whatTheRopeIsConnectedTo.gameObject.AddComponent<SpringJoint>();
        springJoint.enableCollision = true;
        springJoint.maxDistance = maxRopeLength;
        ropeLength = maxRopeLength;

        //Init the spring we use to approximate the rope from point a to b
        UpdateSpring();

        //Add the weight to what the rope is carrying
        springJoint.connectedBody = whatIsHangingFromTheRope.GetComponent<Rigidbody>();
        //whatIsHangingFromTheRope.GetComponent<Rigidbody>().mass = loadMass;
        lineRenderer.enabled = true;
        showRope = true;
        lineRenderer.enabled = true;
        springJoint.breakForce = 10 * 10 * 10;
    }

    public void CutRope()
    {
        showRope = false;
        lineRenderer.enabled = false;

        if (springJoint != null)
        {
            springJoint.breakForce = 0;
        }

        if (springJoint != null)
        {
            Destroy(springJoint);
        }
    }

    public void AddRope()
    {
        maxRopeLength += ropeIncrement;
    }

    public void GetRope()
    {
        canGet = true;
    }

    public void DontGetRope()
    {
        canGet = false;
    }

    public void LetRope()
    {
        canLet = true;
    }

    public void DontLetRope()
    {
        canLet = false;
    }

    public void GettingRope()
    {
        if (ropeLength > minRopeLength)
        {
            ropeLength -= winchSpeed * Time.deltaTime;

            ropeLength = Mathf.Clamp(ropeLength, minRopeLength, maxRopeLength);

            //Need to recalculate the k-value because it depends on the length of the rope
            UpdateSpring();
        }
        else
        {
            canGet = false;
        }
    }

    public void LettingRope()
    {
        if (ropeLength < maxRopeLength)
        {
            ropeLength += winchSpeed * Time.deltaTime;

            ropeLength = Mathf.Clamp(ropeLength, minRopeLength, maxRopeLength);

            //Need to recalculate the k-value because it depends on the length of the rope
            UpdateSpring();
        }
        else
        {
            canGet = false;
        }
    }

    public void Movement(float dir)
    {
        ropeLength += winchSpeed * Time.deltaTime * dir;

        ropeLength = Mathf.Clamp(ropeLength, minRopeLength, maxRopeLength);

        if (springJoint != null)
        {
            UpdateSpring();
        }

    }

    public bool ExistsRope()
    {
        return springJoint != null;
    }

    //Update the spring constant and the length of the spring
    private void UpdateSpring()
    {
        //Someone said you could set this to infinity to avoid bounce, but it doesnt work
        //kRope = float.inf

        //
        //The mass of the rope
        //
        //Density of the wire (stainless steel) kg/m3
        //float density = 7750f;
        //Density of cotton 1540
        float density = 1540f;
        //The radius of the wire
        float radius = 1f;

        float volume = Mathf.PI * radius * radius * ropeLength;

        float ropeMass = volume * density;

        //Add what the rope is carrying
        //ropeMass += loadMass;


        //
        //The spring constant (has to recalculate if the rope length is changing)
        //
        //The force from the rope F = rope_mass * g, which is how much the top rope segment will carry
        float ropeForce = ropeMass * 9.81f;

        //Use the spring equation to calculate F = k * x should balance this force, 
        //where x is how much the top rope segment should stretch, such as 0.01m

        //Is about 146000
        float kRope = ropeForce / 0.01f;

        //print(ropeMass);

        //Add the value to the spring
        springJoint.spring = kRope * 1.0f;
        //springJoint.damper = kRope * 0.8f;
        //springJoint.spring = 1f;
        springJoint.damper = 0f;

        //Update length of the rope
        springJoint.maxDistance = ropeLength;
    }

    //Display the rope with a line renderer
    private void DisplayRope()
    {
        //This is not the actual width, but the width use so we can see the rope
        float ropeWidth = 0.5f;

        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;


        //Update the list with rope sections by approximating the rope with a bezier curve
        //A Bezier curve needs 4 control points
        Vector3 A = whatTheRopeIsConnectedTo.position;
        Vector3 D = whatIsHangingFromTheRope.position;

        //Upper control point
        //To get a little curve at the top than at the bottom
        Vector3 B = A + whatTheRopeIsConnectedTo.up * (-(A - D).magnitude * 0.1f);
        //B = A;

        //Lower control point
        Vector3 C = D + whatIsHangingFromTheRope.up * ((A - D).magnitude * 0.5f);

        //Get the positions
        BezierCurve.GetBezierCurve(A, B, C, D, allRopeSections);


        //An array with all rope section positions
        Vector3[] positions = new Vector3[allRopeSections.Count];

        for (int i = 0; i < allRopeSections.Count; i++)
        {
            positions[i] = allRopeSections[i];
        }

        //Just add a line between the start and end position for testing purposes
        //Vector3[] positions = new Vector3[2];

        //positions[0] = whatTheRopeIsConnectedTo.position;
        //positions[1] = whatIsHangingFromTheRope.position;


        //Add the positions to the line renderer
        lineRenderer.positionCount = positions.Length;

        lineRenderer.SetPositions(positions);
    }
}
