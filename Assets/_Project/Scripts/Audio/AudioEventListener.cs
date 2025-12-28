using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioEventListener : MonoBehaviour
{
	[SerializeField] private AudioEventChannel _audioMasterChannel;
	[SerializeField] private AudioEventChannel _audioChannel;

	private AudioSource _audioSource;
	private float _baseVolume;

	private void Awake()
	{
		if (_audioChannel == null)
		{
			Debug.LogError("Audio Channel not set!", this);

			enabled = false;
			return;
		}

		_audioSource = GetComponent<AudioSource>();

		_baseVolume = _audioSource.volume;

		OnVolumeChange();
		OnMuteSet();
	}

	private void OnEnable()
	{
		_audioMasterChannel.ActionVolumeChange += OnVolumeChange;
		_audioMasterChannel.ActionMute += OnMuteSet;

		_audioChannel.ActionVolumeChange += OnVolumeChange;
		_audioChannel.ActionMute += OnMuteSet;
	}

	private void OnDisable()
	{
		_audioMasterChannel.ActionVolumeChange -= OnVolumeChange;
		_audioMasterChannel.ActionMute -= OnMuteSet;

		_audioChannel.ActionVolumeChange -= OnVolumeChange;
		_audioChannel.ActionMute -= OnMuteSet;
	}

	private void OnVolumeChange()
	{
		_audioSource.volume = _baseVolume * _audioChannel.CurrentVolume * _audioMasterChannel.CurrentVolume;
	}

	private void OnMuteSet()
	{
		bool isAnyMute = _audioMasterChannel.IsMute || _audioChannel.IsMute;
		_audioSource.mute = isAnyMute;
	}

}
