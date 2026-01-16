using System;
using UnityEngine;

public abstract class AIMobBase : MonoBehaviour, IInput
{
	public virtual event Action<float> Moving;

	#pragma warning disable 0067
	public virtual event Action Jumped;
	public virtual event Action<KeyCode> AbilityKeyPressed;
	#pragma warning restore 0067

	public abstract void AIUpdate();

	public virtual void AIEnter()
	{
		enabled = true;
	}

	public virtual void AIExit()
	{
		Moving?.Invoke(0);
		enabled = false;
	}
}
