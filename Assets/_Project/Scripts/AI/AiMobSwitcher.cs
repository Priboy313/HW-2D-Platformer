using System;
using UnityEngine;

public class AiMobSwitcher : MonoBehaviour, IInput
{
	[SerializeField, HideInInspector] private AIMobBase _currentController;

	public AIMobBase CurrentController
	{
		get => _currentController;
		set => SwitchController(value);
	}

	public event Action<float> ActionMove;
	public event Action ActionJump;

	private void Start()
	{
		AIMobBase startController = _currentController;

		_currentController = null;

		if (_currentController != null)
		{
			SwitchController(_currentController);
		}
		else
		{
			AIMobBase controller = GetComponent<AIMobBase>();
			
			if (controller != null)
			{
				SwitchController(controller);
			}
		}
	}

	private void Update()
	{
		if (_currentController != null)
		{
			_currentController.AIUpdate();
		}
	}

	public void SwitchController(AIMobBase newController)
	{
		if (_currentController == newController)
		{
			return;
		}

		if (_currentController != null)
		{
			_currentController.ActionMove -= OnMove;
			_currentController.ActionJump -= OnJump;
			_currentController.AIExit();
		}

		_currentController = newController;

		if (_currentController != null)
		{
			_currentController.AIEnter();
			_currentController.ActionMove += OnMove;
			_currentController.ActionJump += OnJump;
		}
	}

	private void OnMove(float direction)
	{
		ActionMove?.Invoke(direction);
	}

	private void OnJump()
	{
		ActionJump?.Invoke();
	}
}
