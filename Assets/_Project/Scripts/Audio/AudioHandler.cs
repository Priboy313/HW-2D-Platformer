using UnityEngine;


public class AudioHandler : MonoBehaviour
{
	[SerializeField] private AudioEventChannel _audioChannel;

	public void SetVolume(float volume)
	{
		_audioChannel.RaiseVolumeChange(volume);
	}

	public void SetMute(bool isMute)
	{
		_audioChannel.RaiseMute(isMute);
	}
}
