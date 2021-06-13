using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryBehavior : MonoBehaviour
{
    public BatteryController controller;


    private void Start()
    {
        controller = FindObjectOfType<BatteryController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        // make rope longer
        controller.OnBatteryTouch.Invoke();
        Destroy(gameObject, 1f);
    }
}
