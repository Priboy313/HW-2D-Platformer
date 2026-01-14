using UnityEngine;

namespace Abilities {

	public abstract class AbilityBase : MonoBehaviour
	{
		protected InputEventChannel _inputChannel;
		protected IInput _input;
		protected IAbilityOwner _owner;

		public virtual void Init(IAbilityOwner owner, InputEventChannel inputChannel = null)
		{
			bool hasErrors = false;

			if (_inputChannel == null)
			{
				_input = GetComponent<IInput>();

				if (_input == null)
				{
					Debug.LogError("InputChannel not set!");
					hasErrors = true;
				}
			}
			else
			{
				_input = _inputChannel;
			}

			if (hasErrors)
			{
				enabled = false;
				return;
			}

			AddInputListeners();
		}

		protected virtual void AddInputListeners()
		{

		}
	}
}
