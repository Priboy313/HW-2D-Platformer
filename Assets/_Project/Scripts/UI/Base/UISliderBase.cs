using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class UISliderBase : MonoBehaviour
{
	protected Slider _slider;

	protected virtual void Awake()
	{
		_slider = GetComponent<Slider>();
	}

	protected virtual void OnEnable()
	{
		if (_slider != null)
		{
			_slider.onValueChanged.AddListener(OnValueChanged);
		}
	}

	protected virtual void OnDisable()
	{
		_slider.onValueChanged.RemoveListener(OnValueChanged);
	}

	protected abstract void OnValueChanged(float value);
}
