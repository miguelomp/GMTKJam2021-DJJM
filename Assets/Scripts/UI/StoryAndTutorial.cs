using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class StoryAndTutorial : MonoBehaviour
{
    public PlayerInput playerInput;

    private bool storyDisplayed = true;
    private bool tutorialDisplayed = false;
    private IMGUIContainer story;
    private IMGUIContainer tutorial;
    private Button buttonClose;
    private VisualElement root;
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        story = root.Q<IMGUIContainer>("Story");
        tutorial = root.Q<IMGUIContainer>("Tutorial");
        buttonClose = root.Q<Button>("close-modal");
        
        //root.RegisterCallback<KeyDownEvent>(NextModal, TrickleDown.TrickleDown);
        buttonClose.RegisterCallback<ClickEvent>(ev => CloseModal());
    }

    private void CloseModal()
    {
        root.style.display = DisplayStyle.None;
        // TODO: Unpaused the game here 
        
        //root.UnregisterCallback<KeyDownEvent>(NextModal, TrickleDown.TrickleDown);
        buttonClose.UnregisterCallback<ClickEvent>(ev => CloseModal());

        playerInput.SwitchCurrentActionMap("MainControl");
    }

    public void NextModal()
    {
        if (storyDisplayed && !tutorialDisplayed)
        {
            story.style.display = DisplayStyle.None;
            tutorial.style.display = DisplayStyle.Flex;
            tutorialDisplayed = true;
        }
        else if (tutorialDisplayed)
        {

            CloseModal();
        }
    }

    public void NextModal(KeyDownEvent ev)
    {
        if (ev.keyCode != KeyCode.Space)
            return;
        
        if (storyDisplayed && !tutorialDisplayed)
        {
            story.style.display = DisplayStyle.None;
            tutorial.style.display = DisplayStyle.Flex;
            tutorialDisplayed = true;
        } else if (tutorialDisplayed)
            CloseModal();
    }
}
