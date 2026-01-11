using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HealthUISystem {

	[RequireComponent(typeof(Slider))]
	public class UIHealthSliderSmooth : MonoBehaviour, IHealthUI
	{
		[SerializeField] private float _speed = 1f;

		private Slider _slider;
		private Coroutine _coroutineUpdateHealthBar;

		private void Awake()
		{
			_slider = GetComponent<Slider>();
		}

		public void SetHealth(float current, float max)
		{
			float targetValue = Mathf.Clamp01(current / max);

			if (_coroutineUpdateHealthBar != null)
			{
				StopCoroutine(_coroutineUpdateHealthBar);
			}

			_coroutineUpdateHealthBar = StartCoroutine(UpdateHealthBarRoutine(targetValue));
		}
		
		private IEnumerator UpdateHealthBarRoutine(float targetValue)
		{
			while (Mathf.Approximately(_slider.value, targetValue) == false)
			{
				_slider.value = Mathf.MoveTowards(_slider.value, targetValue, _speed * Time.deltaTime);

				yield return null;
			}

			_coroutineUpdateHealthBar = null;
		}
	}
}
