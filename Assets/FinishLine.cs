using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FinishLine : MonoBehaviour
{
    public PlayerInput playerInput;
    public Goodbye chaito;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("YOU WIN");
            chaito.Show();
            playerInput.SwitchCurrentActionMap("ByeControl");
        }
    }
}
