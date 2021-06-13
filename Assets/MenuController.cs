using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class MenuController : MonoBehaviour
{

    public MenuDisplay menu;
    public PlayerInput playerInput;
    public void OnExitMenu(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            menu.HideMenu();
        }
    }

    public void OnEnterMenu(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            menu.ShowMenu();
        }
    }
}
