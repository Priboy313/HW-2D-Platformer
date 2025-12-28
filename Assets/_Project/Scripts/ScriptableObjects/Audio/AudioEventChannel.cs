using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioChannel", menuName = "Audio/AudioChannel")]
public class AudioEventChannel : ScriptableObject
{
	public event Action ActionVolumeChange;
	public event Action ActionMute;

	public float CurrentVolume { get; private set; } = 1f;
	public bool IsMute { get; private set; } = false;

	public void RaiseVolumeChange(float volume)
	{
		CurrentVolume = Mathf.Clamp01(volume);
		ActionVolumeChange?.Invoke();
	}

	public void RaiseMute(bool isMute)
	{
		IsMute = isMute;
		ActionMute?.Invoke();
	}

	private void OnDisable()
	{
		CurrentVolume = 1f;
		IsMute = false;

		ActionVolumeChange = null;
		ActionMute = null;
	}
}
