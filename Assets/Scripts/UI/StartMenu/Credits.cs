using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Credits : MonoBehaviour
{
    private static bool modalOpened = false;
    private Button buttonCredits;
    private Button buttonCloseCredits;
    private IMGUIContainer modalCredits;
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        buttonCloseCredits = root.Q<Button>("close-modal");
        modalCredits = root.Q<IMGUIContainer>("Credits-modal");
        buttonCredits = root.Q<Button>("Credits");
        
        buttonCredits.RegisterCallback<ClickEvent>(ev => toggleModal());
        buttonCloseCredits.RegisterCallback<ClickEvent>(ev => toggleModal());
    }

    private void toggleModal()
    {
        if (modalOpened)
            modalCredits.style.display = DisplayStyle.None;
        else
            modalCredits.style.display = DisplayStyle.Flex;

        modalOpened = !modalOpened;
    }
}
