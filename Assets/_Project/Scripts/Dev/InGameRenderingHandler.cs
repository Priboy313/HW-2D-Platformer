using UnityEngine;

public class InGameRenderingHandler : MonoBehaviour
{
    [SerializeField] private bool _autoHideInAwake = true;

    private InputReader _input;
    private bool isActiveRendering = true;

    private void Awake()
    {
        if (_autoHideInAwake)
        {
            HideAllRenderers();
        }

        _input = InputReader.Instance;

        if (_input == null)
        {
            _input = FindObjectOfType<InputReader>();

            if (_input == null)
            {
                Debug.LogError("InputReader not set!");
                enabled = false;
            }
        }
    }

    private void OnEnable()
    {
        _input.ActionDevRenderStateToggle += OnDevPropRenderingChange;
    }

    private void OnDisable()
    {
        if (_input != null)
        {
            _input.ActionDevRenderStateToggle -= OnDevPropRenderingChange;
        }
    }

    private void HideAllRenderers()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        isActiveRendering = false;
    }

    private void ShowAllRenderers()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = true;
        }

        isActiveRendering = true;
    }

    private void OnDevPropRenderingChange()
    {
        if (isActiveRendering)
        {
            HideAllRenderers();
        }
        else
        {
            ShowAllRenderers();
        }
    }
}
