using UnityEngine;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(AudioSource))]
	public class PauseAudio : MonoBehaviour 
	{
		private AudioSource m_audioSource;
		private bool m_isPaused;

		private void Start()
		{
			m_audioSource = GetComponent<AudioSource>();
			m_isPaused = false;

			if(NotificationCenter.Exists)
			{
				NotificationCenter.AddObserver(gameObject, Notifications.ON_GAME_PAUSED);
				NotificationCenter.AddObserver(gameObject, Notifications.ON_GAME_UNPAUSED);
			}
		}

		private void OnDestroy()
		{
			if(NotificationCenter.Exists)
			{
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_GAME_PAUSED);
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_GAME_UNPAUSED);
			}
		}

		private void OnGamePaused()
		{
			if(m_audioSource.isPlaying)
			{
				m_audioSource.Pause();
				m_isPaused = true;
			}
		}

		private void OnGameUnpaused()
		{
			if(m_isPaused)
			{
				m_audioSource.Play();
				m_isPaused = false;
			}
		}
	}
}