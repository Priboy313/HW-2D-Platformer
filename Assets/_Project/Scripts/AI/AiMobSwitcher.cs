using System;
using UnityEngine;

public class AIMobSwitcher : MonoBehaviour, IInput
{
	[SerializeField, HideInInspector] private AIMobBase _currentController;
	[SerializeField, HideInInspector] private bool _isAggressive;

	private AIMobBase _defaultController;
	private AIMobPursuer _pursuerController;
	private AIMobVisionHandler _vision;

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

	private void Awake()
	{
		if (IsAggressive)
		{
			_pursuerController = GetComponent<AIMobPursuer>();
			_vision = GetComponentInChildren<AIMobVisionHandler>();
		}
	}

	private void Start()
	{
		_defaultController = _currentController;

		if (_defaultController == null)
		{
			var allControllers = GetComponents<AIMobBase>();

			foreach (var controller in allControllers)
			{
				if (controller != _pursuerController)
				{
					_defaultController = controller;
					break;
				}
			}
		}

		_currentController = null;
		
		if (_defaultController != null)
		{
			SwitchController(_defaultController);
		}

		if (IsAggressive)
		{
			if (_vision != null && _pursuerController != null)
			{
				_vision.ActionTargetFound += OnTargetFound;
				_vision.ActionTargetLost += OnTargetLost;
			}
		}
	}

	private void OnDestroy()
	{
		if (_vision != null)
		{
			_vision.ActionTargetFound -= OnTargetFound;
			_vision.ActionTargetLost -= OnTargetLost;
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

	private void OnTargetFound(Character target)
	{
		if (IsAggressive == false)
		{
			return;
		}

		_pursuerController.SetTarget(target);
		SwitchController(_pursuerController);
	}

	private void OnTargetLost()
	{
		if (IsAggressive == false)
		{
			return;
		}

		_pursuerController.SetTarget(null);
		SwitchController(_defaultController);
	}
}
