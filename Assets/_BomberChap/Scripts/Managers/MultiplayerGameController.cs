using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	public class MultiplayerGameController : MonoBehaviour 
	{
		public static bool PlayerOneWon = false;
		public static bool PlayerTwoWon = false;

		[SerializeField]
		private ScreenFader m_screenFader;
		[SerializeField]
		private Canvas m_canvas;
		[SerializeField]
		private Text m_roundText;
		[SerializeField]
		private float m_roundTexDuration;
		[SerializeField]
		private int m_maxRounds;

		private int m_levelIndex;
		private int m_currentRound;
		private int m_playerOneWins;
		private int m_playerTwoWins;
		private bool m_registeredDeathThisRound;

		private void Start()
		{
			m_levelIndex = UnityEngine.Random.Range(0, LevelManager.LevelCount);
			m_currentRound = 0;
			m_playerOneWins = 0;
			m_playerTwoWins = 0;

			NotificationCenter.AddObserver(gameObject, Notifications.ON_PLAYER_ONE_DEAD);
			NotificationCenter.AddObserver(gameObject, Notifications.ON_PLAYER_TWO_DEAD);

			StartCoroutine(StartNextRound());
		}

		private void OnDestroy()
		{
			if(NotificationCenter.Exists)
			{
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_PLAYER_ONE_DEAD);
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_PLAYER_TWO_DEAD);
			}
		}

		private IEnumerator StartNextRound()
		{
			yield return null;

			if(m_currentRound == m_maxRounds)
			{
				PlayerOneWon = m_playerOneWins > m_playerTwoWins;
				PlayerTwoWon = m_playerTwoWins > m_playerOneWins;
				NotificationCenter.Dispatch(Notifications.ON_MULTI_PLAYER_MATCH_OVER);
				yield break;
			}

			PauseManager.Pause();
			if(m_screenFader != null)
				yield return m_screenFader.FadeOut();
			else
				yield return null;

			m_currentRound++;
			m_roundText.text = "ROUND " + m_currentRound;
			m_canvas.gameObject.SetActive(true);
			m_screenFader.ClearFade();
			LevelManager.LoadLevel(m_levelIndex, false);

			float elapsedTime = 0.0f;
			while(elapsedTime < m_roundTexDuration)
			{
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			m_canvas.gameObject.SetActive(false);

			if(m_screenFader != null)
				yield return m_screenFader.FadeIn();
			else
				yield return null;
			
			PauseManager.UnPause();
			m_screenFader.ClearFade();
			m_registeredDeathThisRound = false;
		}

		private void OnPlayerOneDead()
		{
			m_playerTwoWins++;
			if(!m_registeredDeathThisRound)
			{
				m_registeredDeathThisRound = true;
				StartCoroutine(StartNextRound());
			}
		}

		private void OnPlayerTwoDead()
		{
			m_playerOneWins++;
			if(!m_registeredDeathThisRound)
			{
				m_registeredDeathThisRound = true;
				StartCoroutine(StartNextRound());
			}
		}
	}
}