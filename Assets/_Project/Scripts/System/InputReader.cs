using System;
using UnityEngine;

public class InputReader : MonoBehaviour 
{
    private const KeyCode ZoomIn = KeyCode.KeypadPlus;
    private const KeyCode ZoomOut = KeyCode.KeypadMinus;
    private const float ZoomKeyboardSpeed = 1f;
    private const bool IsZoomInvert = false;

    private const string AxisScrollWheel = "Mouse ScrollWheel";
    private const string AxisHorizontal = "Horizontal";
    private const string ButtonJumpName = "Jump";

    public event Action<float> ActionMove;
    public event Action ActionJump;
    public event Action<float> ActionZoomChange;

    private void Update()
    {
        ObserveKeyboard();
        ObserveMouse();
    }

    private void ObserveKeyboard()
    {
        ActionMove?.Invoke(Input.GetAxisRaw(AxisHorizontal));

        if (Input.GetButtonDown(ButtonJumpName))
        {
            ActionJump?.Invoke();
        }

        if (Input.GetKeyDown(ZoomIn))
        {
            ActionZoomChange?.Invoke(IsZoomInvert ? ZoomKeyboardSpeed : -ZoomKeyboardSpeed);
        }

        if (Input.GetKeyDown(ZoomOut))
        {
            ActionZoomChange?.Invoke(IsZoomInvert ? -ZoomKeyboardSpeed : ZoomKeyboardSpeed);
        }
    }

    private void ObserveMouse()
    {
        float scrollWheel = Input.GetAxisRaw(AxisScrollWheel);

        if (scrollWheel != 0)
        {
            ActionZoomChange?.Invoke(IsZoomInvert ? scrollWheel : -scrollWheel);
        }
    }
}