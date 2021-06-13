using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartGame : MonoBehaviour
{
    public String GameScene = "Assets/Scenes/Playroom";
    private VisualElement wrapper;
    private VisualElement root;
    private void OnEnable()
    {
         root = GetComponent<UIDocument>().rootVisualElement;
         wrapper = root.Q<IMGUIContainer>("Wrapper");
         root.RegisterCallback<KeyDownEvent>(InitGame, TrickleDown.TrickleDown);
    }

    private void InitGame(KeyDownEvent ev)
    {
        if (ev.keyCode != KeyCode.Space)
            return;
        
        SceneManager.LoadScene(GameScene);
        root.style.display = DisplayStyle.None;
        root.UnregisterCallback<KeyDownEvent>(InitGame, TrickleDown.TrickleDown);
    }
}
