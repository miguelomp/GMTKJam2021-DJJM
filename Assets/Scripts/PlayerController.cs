using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Sub Behaviours")]
    public BodyMovementBehavior playerMovementBehaviour;
    public HeadBehavior headBehavior;
    public SpinBehavior spinBehavior;
    public CameraBehavior cameraBehavior;
    public RopeBehavior ropeBehavior;
    public BodyMassBehavior bodyMassBehavior;
    public MenuDisplay menu;
    public Goodbye chaito;

    [Header("Input Settings")]
    public PlayerInput playerInput;
    public float movementSmoothingSpeed = 1f;
    private Vector3 rawInputMovement;
    private Vector3 rawInputMovement2;
    private Vector3 smoothInputMovement = Vector3.zero;
    private Vector3 smoothInputMovement2 = Vector3.zero;
    private float rawInputRotation = 0f;
    private float smoothInputRotation = 0f;
    private float rawInputRope = 0f;
    private float smoothInputRope = 0f;

    private string lastActionMap = "";

    float distance = 0f;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerInput.currentActionMap.name);

        CalculateSmoothing();

        UpdateHeadMovement();
        UpdateMainMovement();
        UpdateRopeMovement();

        Vector3 a = bodyMassBehavior.transform.position;
        Vector3 b = headBehavior.transform.position;

        distance = Vector3.Distance(new Vector3(0, a.y, a.z), new Vector3(0, b.y, b.z));
    }

    #region MainActionMap

    //public void OnGoodByeEnter(InputAction.CallbackContext value)
    //{
    //    chaito.Show();
    //    playerInput.SwitchCurrentActionMap("ByeControl");
    //}

    public void ResetGame(InputAction.CallbackContext value)
    {
        chaito.GoToInitScene();
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            playerMovementBehaviour.Jump();
        }
    }

    public void OnSpinAndDetachClockwise(InputAction.CallbackContext value)
    {
        SpinAndDetach(value, spinBehavior.ClockWise);
    }

    public void OnSpinAndDetachAnticlockwise(InputAction.CallbackContext value)
    {
        SpinAndDetach(value, spinBehavior.AntiClockWise);
    }
    #endregion

    #region HeadActionMap & BodyMap
    public void OnRotate(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        rawInputMovement2 = new Vector3(inputMovement.x, 0, inputMovement.y);
        // rotate to one side or the other
    }

    public void OnGoBody(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            playerInput.SwitchCurrentActionMap("BodyControl");
            cameraBehavior.FollowBody();
        }
    }

    public void OnGoMain(InputAction.CallbackContext value)
    {
        //if distance between head a body
        //  redo main body
        //  change to main control
        //Vector3 a = bodyMassBehavior.transform.position;
        //Vector3 b = headBehavior.transform.position;
        //if (Vector3.Distance(new Vector3(0, a.y, a.z), new Vector3(0, b.y, b.z)) < 1.5f)
        //{
        //    ropeBehavior.CutRope();
        //    bodyMassBehavior.SetBody();
        //    playerInput.SwitchCurrentActionMap("MainControl");
        //    headBehavior.HeadOn();
        //}

        ////if rope existe then get it
        //if (value.performed && ropeBehavior.ExistsRope())
        //{
        //    ropeBehavior.GetRope();
        //}
        //if (value.canceled)
        //{
        //    ropeBehavior.DontGetRope();

            
        //}
    }

    public void OnAxisJoin(InputAction.CallbackContext value)
    {
        if (!value.canceled)
        {
            rawInputRope = value.ReadValue<float>();

            if (distance < 1.5f && rawInputRope < 0)
            {
                ropeBehavior.CutRope();
                bodyMassBehavior.SetBody();
                playerInput.SwitchCurrentActionMap("MainControl");
                headBehavior.HeadOn();
            }
        }
    }

    public void OnCutJoin(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (ropeBehavior.ExistsRope())
            {
                ropeBehavior.CutRope();
            }
            else
            {
                ropeBehavior.CreateRope();
            }
        }
    }

    public void OnGoHead(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            playerInput.SwitchCurrentActionMap("HeadControl");
            cameraBehavior.FollowHead();
        }
    }
    #endregion

    public void OnGoMenu(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            menu.ShowMenu();
            lastActionMap = playerInput.currentActionMap.name;
            playerInput.SwitchCurrentActionMap("MenuControl");
            // PAUSAR EL JUEGO ?
        }
    }

    public void OnGoGame(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            menu.HideMenu();
            playerInput.SwitchCurrentActionMap(lastActionMap);
        }
        
    }

    #region helpers
    void CalculateSmoothing()
    {

        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
        smoothInputMovement2 = Vector3.Lerp(smoothInputMovement2, rawInputMovement2, Time.deltaTime * movementSmoothingSpeed);
        smoothInputRotation = Mathf.Lerp(smoothInputRotation, rawInputRotation, Time.deltaTime * movementSmoothingSpeed);
        smoothInputRope = Mathf.Lerp(smoothInputRope, rawInputRope, Time.deltaTime * movementSmoothingSpeed);

    }

    void UpdateMainMovement()
    {
        if (playerMovementBehaviour.IsGrounded())
        {
            playerMovementBehaviour.UpdateMovementData(smoothInputMovement);
        }
    }

    void UpdateHeadMovement()
    {
        headBehavior.UpdateMovementData(smoothInputRotation, smoothInputMovement2);
    }

    void UpdateRopeMovement()
    {
        ropeBehavior.Movement(smoothInputRope);
    }

    public void DetachHead()
    {
        spinBehavior.Detach();
        //cameraBehavior.FollowHead();
        // trigger animation
        // change to head control
        playerInput.SwitchCurrentActionMap("HeadControl");
        // create rope
        ropeBehavior.CreateRope();
        // 
    }

    private void SpinAndDetach(InputAction.CallbackContext value, Action spinnAction)
    {
        headBehavior.HeadOff();

        if (value.started)
        {
            // trigger animation
            spinnAction();
            spinBehavior.StartMotor();
        }
        if (value.canceled)
        {
            // detach head
            DetachHead();
        }
    }
    public void SpinAndDetachRandom()
    {
        if (playerInput.currentActionMap.name == "MainControl")
        {
            StartCoroutine(SpinAndDetachCourutine());
        }
    }

    IEnumerator SpinAndDetachCourutine()
    {
        spinBehavior.ClockWise();
        spinBehavior.StartMotor();
        spinBehavior.Detach();
        playerInput.SwitchCurrentActionMap("Empty");
        yield return new WaitForSeconds(.5f);
        playerInput.SwitchCurrentActionMap("HeadControl");

        yield return null;
    }
    #endregion
}
