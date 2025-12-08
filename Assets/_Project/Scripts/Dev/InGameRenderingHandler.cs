using UnityEngine;

public class InGameRenderingHandler : MonoBehaviour
{
    [SerializeField] private bool _autoHideInAwake = true;
	[SerializeField] private InputEventChannel _inputChannel;

    private bool isActiveRendering = true;

    private void Awake()
    {
        if (_autoHideInAwake)
        {
            HideAllRenderers();
        }

        if (_inputChannel == null)
        {
            Debug.LogError("InputReader not set!");
            enabled = false;
        }
    }

    private void OnEnable()
    {
        _inputChannel.ActionDevRenderStateToggle += OnDevPropRenderingChange;
    }

    private void OnDisable()
    {
        if (_inputChannel != null)
        {
            _inputChannel.ActionDevRenderStateToggle -= OnDevPropRenderingChange;
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
