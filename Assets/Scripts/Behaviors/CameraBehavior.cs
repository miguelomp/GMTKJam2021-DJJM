using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraBehavior : MonoBehaviour
{
    public Transform head;
    public Transform body;

    CinemachineVirtualCamera cam;

    bool followHead = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void FollowHead()
    {
        cam.Follow = head;
    }

    public void FollowBody()
    {
        cam.Follow = body;
    }
}
