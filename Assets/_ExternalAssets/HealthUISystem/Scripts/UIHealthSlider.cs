using UnityEngine;
using UnityEngine.UI;

namespace HealthUISystem {
	
	[RequireComponent(typeof(Slider))]
	public class UIHealthSlider : MonoBehaviour, IHealthUI
	{
		private Slider _slider;

		private void Awake()
		{
			_slider = GetComponent<Slider>();
		}

		public void SetHealth(float current, float max)
		{
			float value = Mathf.Clamp01(current / max);
			_slider.value = value;
		}
	}
}
