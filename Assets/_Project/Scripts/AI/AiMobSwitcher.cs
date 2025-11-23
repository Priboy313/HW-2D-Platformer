using System;
using UnityEngine;

public class AIMobSwitcher : MonoBehaviour, IInput
{
	[SerializeField, HideInInspector] private AIMobBase _currentController;
	[SerializeField, HideInInspector] private bool _isAggressive;

	public AIMobBase CurrentController
	{
		get => _currentController;
		set => SwitchController(value);
	}

	public bool IsAggressive
	{
		get => _isAggressive;
		set => _isAggressive = value;
	}

	public event Action<float> ActionMove;
	public event Action ActionJump;

	private void Start()
	{
		AIMobBase startController = _currentController;

		_currentController = null;

		if (startController != null)
		{
			SwitchController(startController);
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
