using System;
using UnityEngine;

public class AIMobPursuer : AIMobBase 
{
	[Header("Vision Settings")]
	[SerializeField, Min(0)] private float _width = 10f;
	[SerializeField, Min(0)] private float _height = 3f;
	[SerializeField] private float _offsetVertical = 0.8f;

	private AIMobVisionHandler _vision;
	private Character _currentTarget;

	public override event Action<float> ActionMove;

	private void OnValidate()
	{
		if (_vision == null)
		{
			_vision = GetComponentInChildren<AIMobVisionHandler>();
		}
		else
		{
			_vision.UpdateColliderSettings(_width, _height, _offsetVertical);
		}
	}

	private void Awake()
	{
		_vision = GetComponentInChildren<AIMobVisionHandler>();

		if (_vision != null)
		{
			_vision.Init();
			_vision.UpdateColliderSettings(_width, _height, _offsetVertical);
		}
		else
		{
			Debug.LogError("Vision handler not found!", this);
			enabled = false;
		}

		enabled = false;
	}

	public void SetTarget(Character target)
	{
		_currentTarget = target;
	}

	public override void AIUpdate()
	{
		if (_currentTarget == null)
		{
			ActionMove?.Invoke(0);
			return;
		}

		float direction = Mathf.Sign(_currentTarget.transform.position.x - transform.position.x);
		ActionMove?.Invoke(direction);
	}
}
