using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ShockBehavior : MonoBehaviour
{
    public ShockController controller;

    float shockThreshold = .5f;
    float shockDeath = 1f;

    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<ShockController>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        timer = 0;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        if (timer > shockDeath)
        {
            // call death
            Debug.Log("call death");
            return;
        }

        if (timer > shockThreshold)
        {
            // call Shock
            controller.OnShortShock.Invoke();
            return;
        }


    }
}
