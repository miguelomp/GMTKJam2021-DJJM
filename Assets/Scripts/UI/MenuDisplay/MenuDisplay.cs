using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuDisplay : MonoBehaviour
{

    private bool menuDisplayed = false;
    private VisualElement menuWrapper;
    private VisualElement root;
    private Button resumeButton;
    private Button exitButton;
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        menuWrapper = root.Q<IMGUIContainer>("Wrapper");
        resumeButton = root.Q<Button>("Resume");
        exitButton = root.Q<Button>("Quit");

        resumeButton.RegisterCallback<ClickEvent>(ev => toggleMenu());
        exitButton.RegisterCallback<ClickEvent>(ev => Application.Quit());
        root.style.display = DisplayStyle.None;
    }
    
    public void toggleMenu()
    {
        if (menuDisplayed)
            root.style.display = DisplayStyle.None;
        else
            root.style.display = DisplayStyle.Flex;

        menuDisplayed = !menuDisplayed;
    }

    public void ShowMenu()
    {
        root.style.display = DisplayStyle.Flex;
    }

    public void HideMenu()
    {
        root.style.display = DisplayStyle.None;
    }
}
