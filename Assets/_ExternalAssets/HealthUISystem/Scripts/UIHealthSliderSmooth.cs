using UnityEngine;
using UnityEngine.UI;

namespace HealthUISystem {

	[RequireComponent(typeof(Slider))]
	public class UIHealthSliderSmooth : MonoBehaviour, IHealthUI
	{
		[SerializeField] private float _speed = 1f;

		private Slider _slider;
		private float _targetValue = 1f;

		private void Awake()
		{
			_slider = GetComponent<Slider>();
		}

		private void Update()
		{
			if (Mathf.Approximately(_slider.value, _targetValue) == false)
			{
				_slider.value = Mathf.MoveTowards(_slider.value, _targetValue, _speed * Time.deltaTime);
			}
		}

		public void SetHealth(float current, float max)
		{
			_targetValue = Mathf.Clamp01(current / max);
		}
	}
}
