using System;
using UnityEngine;

public class InputReader : MonoBehaviour, IInput
{
    private static InputReader _instance;

    private const KeyCode KeyChangeDevPropRendering = KeyCode.KeypadMultiply;

    private const KeyCode KeyZoomIn = KeyCode.KeypadPlus;
    private const KeyCode KeyZoomOut = KeyCode.KeypadMinus;
    private const float ZoomKeyboardSpeed = 1f;
    private const bool IsZoomInvert = false;

    private const string AxisScrollWheel = "Mouse ScrollWheel";
    private const string AxisHorizontal = "Horizontal";
    private const string ButtonJumpName = "Jump";

    public event Action<float> ActionMove;
    public event Action ActionJump;
    public event Action<float> ActionZoomChange;
    public event Action ActionDevRenderStateToggle;

    public static InputReader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputReader>();
                
                if (_instance == null)
                {
                    GameObject newInputReader = new GameObject(nameof(InputReader));
                    _instance = newInputReader.AddComponent<InputReader>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

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

        if (Input.GetKeyDown(KeyZoomIn))
        {
            ActionZoomChange?.Invoke(IsZoomInvert ? ZoomKeyboardSpeed : -ZoomKeyboardSpeed);
        }

        if (Input.GetKeyDown(KeyZoomOut))
        {
            ActionZoomChange?.Invoke(IsZoomInvert ? -ZoomKeyboardSpeed : ZoomKeyboardSpeed);
        }

        if (Input.GetKeyDown(KeyChangeDevPropRendering))
        {
            ActionDevRenderStateToggle?.Invoke();
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