using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	public class MultiplayerGameController : MonoBehaviour 
	{
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
		[SerializeField]
		private float m_nextRoundDelay;

		private int m_levelIndex;
		private int m_currentRound;
		private int m_playerOneWins;
		private int m_playerTwoWins;
		private bool m_registeredDeathThisRound;

		public int MaxRounds
		{
			get { return m_maxRounds; }
		}

		public int PlayerOneWins
		{
			get { return m_playerOneWins; }
		}

		public int PlayerTwoWins
		{
			get { return m_playerTwoWins; }
		}

		private void Start()
		{
			m_levelIndex = UnityEngine.Random.Range(0, LevelManager.LevelCount);
			m_currentRound = 0;
			m_playerOneWins = 0;
			m_playerTwoWins = 0;

			StartCoroutine(StartNextRound(0.0f));
		}

		private IEnumerator StartNextRound(float delay)
		{
			if(delay > 0.0f)
				yield return new WaitForSeconds(delay);
			else
				yield return null;

			if(m_currentRound == m_maxRounds || m_playerOneWins > m_maxRounds / 2 || m_playerTwoWins > m_maxRounds / 2)
			{
				Globals.SetBool(GlobalKeys.PLAYER_ONE_WON, m_playerOneWins > m_playerTwoWins);
				Globals.SetBool(GlobalKeys.PLAYER_TWO_WON, m_playerTwoWins > m_playerOneWins);
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
				StartCoroutine(StartNextRound(m_nextRoundDelay));
			}
		}

		private void OnPlayerTwoDead()
		{
			m_playerOneWins++;
			if(!m_registeredDeathThisRound)
			{
				m_registeredDeathThisRound = true;
				StartCoroutine(StartNextRound(m_nextRoundDelay));
			}
		}
	}
}