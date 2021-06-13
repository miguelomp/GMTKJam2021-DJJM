using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class TutoController : MonoBehaviour
{
    public StoryAndTutorial storyAndTutorial;
    public PlayerInput playerInput;
    public void OnContinue(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            storyAndTutorial.NextModal();
        }
    }
}
