using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class LevelResetTimer : MonoBehaviour 
	{
		[SerializeField]
		private AudioSource m_timerSoundSource;

		[SerializeField]
		private float m_timeToPlayTimerSound;

		private float m_remainingTime;

		public float RemainingTime
		{
			get { return m_remainingTime; }
		}

		private void OnGameLevelLoaded()
		{
			StopAllCoroutines();
			m_remainingTime = LevelManager.GetLoadedLevel().AllocatedTime;

			StartCoroutine(StartTimer());
		}

		private IEnumerator StartTimer()
		{
			bool startedTimerSound = false;

			while(m_remainingTime > 0.0f)
			{
				if(m_remainingTime <= m_timeToPlayTimerSound && m_timerSoundSource != null && !startedTimerSound)
				{
					m_timerSoundSource.Play();
					startedTimerSound = true;
				}

				m_remainingTime -= Time.deltaTime;
				if(m_remainingTime < 0.0f)
					m_remainingTime = 0.0f;
				yield return null;
			}

			if(m_timerSoundSource != null && m_timerSoundSource.isPlaying)
				m_timerSoundSource.Stop();
			
			yield return null;
			LevelManager.ReloadCurrentLevel();
		}

		private void OnLastGameLevelComplete()
		{
			if(m_timerSoundSource != null && m_timerSoundSource.isPlaying)
				m_timerSoundSource.Stop();
			StopAllCoroutines();
		}

		private void OnGameLevelComplete()
		{
			if(m_timerSoundSource != null && m_timerSoundSource.isPlaying)
				m_timerSoundSource.Stop();
			StopAllCoroutines();
		}
	}
}