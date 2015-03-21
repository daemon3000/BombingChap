using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	public class SinglePlayerInterface : MonoBehaviour 
	{
		[SerializeField]
		private Canvas m_canvas;
		[SerializeField]
		private Text m_scoreText;
		[SerializeField]
		private Text m_scoreDiffText;
		[SerializeField]
		private Text m_powerupText;
		[SerializeField]
		private Text m_timerText;
		[SerializeField]
		private LevelResetTimer m_timer;
		[SerializeField]
		private int m_timerDangerLevel;
		[SerializeField]
		private float m_scorePointsPerSecond;

		private Animator m_scoreDiffTextAnimator;
		private Animator m_powerupTextAnimator;
		private Animator m_timerTextAnimator;
		private int m_lastRemainingTime;

		private void Start()
		{
			m_scoreDiffTextAnimator = m_scoreDiffText.GetComponent<Animator>();
			m_powerupTextAnimator = m_powerupText.GetComponent<Animator>();
			m_timerTextAnimator = m_timerText.GetComponent<Animator>();

			NotificationCenter.AddObserver(gameObject, Notifications.ON_GAME_LEVEL_LOADED);
			NotificationCenter.AddObserver(gameObject, Notifications.ON_POWERUP_USED);
			NotificationCenter.AddObserver(gameObject, Notifications.ON_SCORE_CHANGED);
		}

		private void OnDestroy()
		{
			if(NotificationCenter.Exists)
			{
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_GAME_LEVEL_LOADED);
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_POWERUP_USED);
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_SCORE_CHANGED);
			}
		}

		private void Update()
		{
			if((int)m_timer.RemainingTime != m_lastRemainingTime)
			{
				if((int)m_timer.RemainingTime <= m_timerDangerLevel && m_lastRemainingTime > m_timerDangerLevel)
				{
					if(m_timerTextAnimator != null)
						m_timerTextAnimator.SetTrigger("Danger");
				}
				m_lastRemainingTime = (int)m_timer.RemainingTime;
				m_timerText.text = "TIME: " + m_lastRemainingTime;
			}
		}

		private void OnGameLevelLoaded()
		{
			m_lastRemainingTime = LevelManager.GetLoadedLevel().AllocatedTime + 1;
			m_timerText.text = "TIME: " + m_lastRemainingTime;
			if(m_timerTextAnimator != null)
			{
				if(m_lastRemainingTime > m_timerDangerLevel)
					m_timerTextAnimator.SetTrigger("Idle");
				else
					m_timerTextAnimator.SetTrigger("Danger");
			}

			m_scoreText.text = "SCORE: " + SinglePlayerGameController.Score;
		}

		private void OnPowerupUsed(object arg)
		{
			PowerupEvent evt = (PowerupEvent)arg;
			PlayerStats playerStats = evt.target.GetComponent<PlayerStats>();

			switch (evt.effect) 
			{
			case PowerupEffect.BombCountUp:
				m_powerupText.text = string.Format("BOMBS: +1({0})", playerStats.MaxBombs);
				break;
			case PowerupEffect.BombCountDown:
				m_powerupText.text = string.Format("BOMBS: -1({0})", playerStats.MaxBombs);
				break;
			case PowerupEffect.BombRangeUp:
				m_powerupText.text = "RANGE: +1";
				break;
			case PowerupEffect.BombRangeDown:
				m_powerupText.text = "RANGE: -1";
				break;
			case PowerupEffect.SpeedUp:
				m_powerupText.text = "SPEED: +1";
				break;
			case PowerupEffect.SpeedDown:
				m_powerupText.text = "SPEED: -1";
				break;
			}

			if(m_powerupTextAnimator != null)
				m_powerupTextAnimator.SetTrigger("Play");
		}

		private void OnScoreChanged(object arg)
		{
			ScoreEvent evt = (ScoreEvent)arg;

			m_scoreDiffText.text = string.Format("{0}{1}", evt.difference >= 0 ? "+" : "", evt.difference);
			if(m_scoreDiffTextAnimator != null)
				m_scoreDiffTextAnimator.SetTrigger("Play");

			m_scoreText.text = "SCORE: " + evt.currentScore;
		}
	}
}