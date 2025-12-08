using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraHandler : MonoBehaviour
{
	[SerializeField] private InputEventChannel _inputChannel;

	[Header("Following")]
	[SerializeField] private Transform _followedObject;
	[SerializeField] private float _speed = 2f;

	[Header("Zoom")]
	[SerializeField] private float _zoomMin = 4f;
	[SerializeField] private float _zoomMax = 10f;
	[SerializeField] private float _zoomSpeed = 0.2f;
	[SerializeField] private float _zoomCurrent = 6f;

	private Camera _camera;

    private void OnValidate()
    {
		if (_zoomCurrent < _zoomMin)
		{
			_zoomCurrent = _zoomMin;
		}

		if (_zoomCurrent > _zoomMax)
		{
			_zoomCurrent = _zoomMax;
		}
    }

    private void Awake()
    {
        if (_inputChannel == null)
        {
            Debug.LogError("InputReader not set!");
            enabled = false;
        }

        _camera = GetComponent<Camera>();

        if (!_camera.orthographic)
        {
            Debug.LogWarning("Камера не является ортографической! Масштабирование может работать не так, как ожидается.", this);
        }
    }

    private void OnEnable()
    {
        _inputChannel.ActionZoomChange += OnZoomChange;
    }

    private void OnDisable()
    {
		if (_inputChannel != null)
		{
			_inputChannel.ActionZoomChange -= OnZoomChange;
		}
    }

    private void Start()
    {
		_camera.orthographicSize = _zoomCurrent;

        if (_followedObject == null)
        {
            return;
        }

        transform.position = new Vector3(_followedObject.position.x, _followedObject.position.y, transform.position.z);
    }

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

	private void OnZoomChange(float change)
	{
		float sizeCurrent = _camera.orthographicSize;

        sizeCurrent += change * _zoomSpeed;

		if (sizeCurrent < _zoomMin)
		{
			sizeCurrent = _zoomMin;
		}

		if (sizeCurrent > _zoomMax)
		{
			sizeCurrent = _zoomMax;
		}

		_zoomCurrent = sizeCurrent;
		_camera.orthographicSize = _zoomCurrent;
	}
}
