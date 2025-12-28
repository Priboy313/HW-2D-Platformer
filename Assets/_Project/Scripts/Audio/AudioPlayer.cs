using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
	[SerializeField] AudioSource _audioSource;
	[SerializeField] AudioClip _audioClip;

	private void Awake()
	{
		bool hasError = false;

		if (_audioSource == null)
		{
			Debug.LogError("Audio Source not set!");
			hasError = true;
		}

		if (_audioClip == null)
		{
			Debug.LogError("Audio Clip not set!");
			hasError = true;
		}

		if (hasError)
		{
			enabled = false;
			return;
		}
	}

	public void PlayAudio()
	{
		_audioSource.clip = _audioClip;
		_audioSource.Play();
	}
}
