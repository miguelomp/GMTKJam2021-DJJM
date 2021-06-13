using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Goodbye : MonoBehaviour
{
    private Button exitButton;
    public string initScene = "CrissScene2";
    private VisualElement root;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        exitButton = root.Q<Button>("close-modal");
        
        exitButton.RegisterCallback<ClickEvent>(ev => SceneManager.LoadScene(initScene));
        //root.RegisterCallback<KeyDownEvent>(GoToInitScene, TrickleDown.TrickleDown);

        Hide();
    }

    public void GoToInitScene()
    {
        SceneManager.LoadScene(initScene);
    }

    private void GoToInitScene(KeyDownEvent ev)
    {
        if (ev.keyCode != KeyCode.Space)
            return;
        
        SceneManager.LoadScene(initScene);
    }

    public void Show()
    {
        root.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        root.style.display = DisplayStyle.None;
    }
}
