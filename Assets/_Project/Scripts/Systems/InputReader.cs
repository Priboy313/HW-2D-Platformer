using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private InputEventChannel _inputChannel;

    private const KeyCode KeyChangeDevPropRendering = KeyCode.KeypadMultiply;

    private const KeyCode KeyZoomIn = KeyCode.KeypadPlus;
    private const KeyCode KeyZoomOut = KeyCode.KeypadMinus;
    private const float ZoomKeyboardSpeed = 1f;
    private const bool IsZoomInvert = false;

    private const string AxisScrollWheel = "Mouse ScrollWheel";
    private const string AxisHorizontal = "Horizontal";
    private const string ButtonJumpName = "Jump";

	private void Awake()
	{
		if (_inputChannel == null)
        {
            Debug.LogError("Input Channel not set!", this);
            enabled = false;
        }
	}

	private void Update()
    {
        ObserveKeyboard();
        ObserveMouse();
    }

    private void ObserveKeyboard()
    {
		_inputChannel.RaiseMove(Input.GetAxisRaw(AxisHorizontal));

        if (Input.GetButtonDown(ButtonJumpName))
        {
            _inputChannel.RaiseJump();

		}

        if (Input.GetKeyDown(KeyZoomIn))
        {
			_inputChannel.RaiseZoomChange(IsZoomInvert ? ZoomKeyboardSpeed : -ZoomKeyboardSpeed);
        }

        if (Input.GetKeyDown(KeyZoomOut))
        {
            _inputChannel.RaiseZoomChange(IsZoomInvert ? -ZoomKeyboardSpeed : ZoomKeyboardSpeed);
        }

        if (Input.GetKeyDown(KeyChangeDevPropRendering))
        {
            _inputChannel.RaiseDevRenderStateToggle();
        }
    }

    private void ObserveMouse()
    {
        float scrollWheel = Input.GetAxisRaw(AxisScrollWheel);

        if (scrollWheel != 0)
        {
            _inputChannel.RaiseZoomChange(IsZoomInvert ? scrollWheel : -scrollWheel);
        }
    }
}