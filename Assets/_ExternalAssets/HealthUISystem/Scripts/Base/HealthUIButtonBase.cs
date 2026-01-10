using UnityEngine;
using UnityEngine.UI;

namespace HealthUISystem.Internal {

	[RequireComponent(typeof(Button))]
	public abstract class HealthUIButtonBase : MonoBehaviour
	{
		protected Button _button;

		protected virtual void Awake()
		{
			_button = GetComponent<Button>();
		}

		protected virtual void OnEnable()
		{
			if (_button != null)
			{
				_button.onClick.AddListener(OnClick);
			}
		}

		protected virtual void OnDisable()
		{
			_button.onClick.RemoveListener(OnClick);
		}

		protected abstract void OnClick();
	}
}
