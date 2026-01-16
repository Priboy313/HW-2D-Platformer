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

    private const KeyCode AbilityBind1 = KeyCode.Q;
	private const KeyCode AbilityBind2 = KeyCode.W;
	private const KeyCode AbilityBind3 = KeyCode.E;
	private const KeyCode AbilityBind4 = KeyCode.R;

	private const string AxisScrollWheel = "Mouse ScrollWheel";
    private const string AxisHorizontal = "Horizontal";
    private const string ButtonJump = "Jump";

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
        ObserveMovementBinds();
        ObserveAbilityBinds();
    }

    private void ObserveMovementBinds()
    {
		_inputChannel.RaiseMove(Input.GetAxisRaw(AxisHorizontal));

		if (Input.GetButtonDown(ButtonJump))
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

    private void ObserveAbilityBinds()
    {
        if (Input.GetKeyDown(AbilityBind1))
        {
            _inputChannel.RaiseAbilityKeyPressed(AbilityBind1);
        }

		if (Input.GetKeyDown(AbilityBind2))
		{
			_inputChannel.RaiseAbilityKeyPressed(AbilityBind2);
		}

		if (Input.GetKeyDown(AbilityBind3))
		{
			_inputChannel.RaiseAbilityKeyPressed(AbilityBind3);
		}

		if (Input.GetKeyDown(AbilityBind4))
		{
			_inputChannel.RaiseAbilityKeyPressed(AbilityBind4);
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