using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InputChannel", menuName = "Gameplay/InputChannel")]
public class InputEventChannel : ScriptableObject, IInput
{
	public event Action<float> Moving;
	public event Action Jumped;

	public event Action<float> ZoomChanged;

	public event Action<KeyCode> AbilityKeyPressed;

	public event Action DevRenderStateToggled;

	public void RaiseMove(float direction)
	{
		Moving?.Invoke(direction);
	}

	public void RaiseJump()
	{
		Jumped?.Invoke();
	}

	public void RaiseZoomChange(float zoom)
	{
		ZoomChanged?.Invoke(zoom);
	}

	public void RaiseAbilityKeyPressed(KeyCode key)
	{
		AbilityKeyPressed?.Invoke(key);
	}

	public void RaiseDevRenderStateToggle()
	{
		DevRenderStateToggled?.Invoke();
	}

	private void OnDisable()
	{
		Moving = null;
		Jumped = null;
		ZoomChanged = null;
		AbilityKeyPressed = null;
		DevRenderStateToggled = null;
	}
}
