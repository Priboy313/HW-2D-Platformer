using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraHandler : MonoBehaviour
{
	[SerializeField] private Transform _followedObject;
	[SerializeField] private float _speed = 2f;
	
	void LateUpdate()
	{
		if (_followedObject == null)
		{
			return;
		}

		transform.position = Vector3.Lerp(
			transform.position, 
			new Vector3(_followedObject.position.x, _followedObject.position.y, transform.position.z), 
			_speed * Time.deltaTime
		);
	}
}
