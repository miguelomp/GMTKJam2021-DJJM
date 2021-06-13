using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Exit : MonoBehaviour
{
    private Button buttonExit;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        buttonExit = root.Q<Button>("Exit");
        
        buttonExit.RegisterCallback<ClickEvent>(ev => ExitGame());
    }

    private void ExitGame()
    {
        buttonExit.UnregisterCallback<ClickEvent>(ev => ExitGame());
        Application.Quit();
    }
}
