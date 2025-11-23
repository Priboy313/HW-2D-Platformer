using System;
using UnityEngine;

public abstract class AIMobBase : MonoBehaviour, IInput
{
	public virtual event Action<float> ActionMove;

	#pragma warning disable 0067
	public virtual event Action ActionJump;
	#pragma warning restore 0067

	public abstract void AIUpdate();

	public virtual void AIEnter()
	{
		enabled = true;
	}

	public virtual void AIExit()
	{
		ActionMove?.Invoke(0);
		enabled = false;
	}
}
