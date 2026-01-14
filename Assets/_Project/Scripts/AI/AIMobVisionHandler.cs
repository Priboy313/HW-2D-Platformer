using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AIMobVisionHandler : MonoBehaviour
{
	private BoxCollider2D _visionTrigger;

	public event Action<Character> TargetFound;
	public event Action TargetLost;

	public void Init()
	{
		_visionTrigger = GetComponent<BoxCollider2D>();

		if (_visionTrigger.isTrigger == false)
		{
			Debug.LogWarning("Vision collider must be Trigger!", this);
			_visionTrigger.isTrigger = true;
		}
	}

	public void UpdateColliderSettings(float width, float height, float offsetVertical)
	{
		if (_visionTrigger == null)
		{
			_visionTrigger = GetComponent<BoxCollider2D>();
		}

		if (_visionTrigger == null)
		{
			return;
		}

		_visionTrigger.offset = new Vector2(_visionTrigger.offset.x, offsetVertical);
		_visionTrigger.size = new Vector2(width, height);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Player target = collision.GetComponentInParent<Player>();

		if (target != null)
		{
			TargetFound?.Invoke(target);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Player target = collision.GetComponentInParent<Player>();

		if (target != null)
		{
			TargetLost?.Invoke();
		}
	}
}
