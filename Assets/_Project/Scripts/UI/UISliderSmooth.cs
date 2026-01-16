using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UISliderSmooth : MonoBehaviour {
	[SerializeField] private float _speed = 1f;

	private Slider _slider;
	private Coroutine _coroutineUpdateHealthBar;

	private void Awake()
	{
		_slider = GetComponent<Slider>();
	}

	public void SetValue(float current, float max)
	{
		float targetValue = Mathf.Clamp01(current / max);

		if (_coroutineUpdateHealthBar != null)
		{
			StopCoroutine(_coroutineUpdateHealthBar);
		}

		_coroutineUpdateHealthBar = StartCoroutine(UpdateValueRoutine(targetValue));
	}

	private IEnumerator UpdateValueRoutine(float targetValue)
	{
		while (Mathf.Approximately(_slider.value, targetValue) == false)
		{
			_slider.value = Mathf.MoveTowards(_slider.value, targetValue, _speed * Time.deltaTime);

			yield return null;
		}

		_coroutineUpdateHealthBar = null;
	}
}
