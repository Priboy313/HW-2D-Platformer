using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InputChannel", menuName = "Gameplay/Input/InputChannel")]
public class InputEventChannel : ScriptableObject, IInput
{
	public event Action<float> ActionMove;
	public event Action ActionJump;
	public event Action<float> ActionZoomChange;
	public event Action ActionDevRenderStateToggle;

	public void RaiseMove(float direction)
	{
		ActionMove?.Invoke(direction);
	}

	public void RaiseJump()
	{
		ActionJump?.Invoke();
	}

	public void RaiseZoomChange(float zoom)
	{
		ActionZoomChange?.Invoke(zoom);
	}

	public void RaiseDevRenderStateToggle()
	{
		ActionDevRenderStateToggle?.Invoke();
	}

	private void OnDisable()
	{
		ActionMove = null;
		ActionJump = null;
		ActionZoomChange = null;
		ActionDevRenderStateToggle = null;
	}
}
