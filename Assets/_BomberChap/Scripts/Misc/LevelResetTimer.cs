using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class LevelResetTimer : MonoBehaviour 
	{
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
			while(m_remainingTime > 0.0f)
			{
				m_remainingTime -= Time.deltaTime;
				if(m_remainingTime < 0.0f)
					m_remainingTime = 0.0f;
				yield return null;
			}
			
			yield return null;
			LevelManager.ReloadCurrentLevel();
		}

		private void OnLastGameLevelComplete()
		{
			StopAllCoroutines();
		}

		private void OnGameLevelComplete()
		{
			StopAllCoroutines();
		}
	}
}