using TMPro;
using UnityEngine;

namespace HealthUISystem {

	[RequireComponent(typeof(TextMeshProUGUI))]
	public class UIHealthText : MonoBehaviour, IHealthUI
	{
		private TextMeshProUGUI _textMesh;
		private string _template = "Health:";

		private void Awake()
		{
			_textMesh = GetComponent<TextMeshProUGUI>();
		}

		public void SetHealth(float current, float max)
		{
			_textMesh.text = $"{_template} {current}/{max}";
		}
	}
}
