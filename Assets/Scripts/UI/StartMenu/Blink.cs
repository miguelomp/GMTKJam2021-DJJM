using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Blink : MonoBehaviour
{
    private bool isBlinking = false;
    public float interval = 10f;
    public float startDelay = 10.5f;

    private bool _toggle;
    
    // Elements
    private IMGUIContainer pressSpaceToStart;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        pressSpaceToStart = root.Q<IMGUIContainer>("press-space");

        _toggle = false;
        startBlink();

    }

    private void startBlink()
    {
        if(isBlinking)
            return;

        isBlinking = true;
        InvokeRepeating("blink", startDelay, interval);
    }
    private void blink()
    {
        Debug.Log("Hello");
        if (_toggle)
            pressSpaceToStart.style.opacity = 100;
        else
            pressSpaceToStart.style.opacity = 0;

        _toggle = !_toggle;
    }
}
