using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class ScoreEvent
	{
		public readonly int previousScore;
		public readonly int currentScore;
		public readonly int difference;
		public readonly int realDifference;

		public ScoreEvent(int previousScore, int currentScore, int difference)
		{
			this.previousScore = previousScore;
			this.currentScore = currentScore;
			this.difference = difference;
			this.realDifference = currentScore - previousScore;
		}
	}

	public class SinglePlayerGameController : MonoBehaviour 
	{
		public static int Score = 0;

		private int m_currentScore = 0;

		private void Start()
		{
			Score = PlayerPrefs.GetInt(PlayerPrefsKeys.SCORE, 0);
			m_currentScore = Score;

			NotificationCenter.AddObserver(gameObject, Notifications.ON_POWERUP_USED);
			NotificationCenter.AddObserver(gameObject, Notifications.ON_GAME_LEVEL_LOADED);
			NotificationCenter.AddObserver(gameObject, Notifications.ON_GAME_LEVEL_COMPLETE);
			NotificationCenter.AddObserver(gameObject, Notifications.ON_LAST_GAME_LEVEL_COMPLETE);
			NotificationCenter.AddObserver(gameObject, Notifications.ON_ENEMY_DEAD);
		}

		private void OnDestroy()
		{
			if(NotificationCenter.Exists)
			{
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_POWERUP_USED);
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_GAME_LEVEL_LOADED);
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_GAME_LEVEL_COMPLETE);
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_LAST_GAME_LEVEL_COMPLETE);
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_ENEMY_DEAD);
			}
		}

		private void OnGameLevelLoaded()
		{
			m_currentScore = Score;
		}

		private void OnGameLevelComplete()
		{
			Score = m_currentScore;
			PlayerPrefs.SetInt(PlayerPrefsKeys.SCORE, Score);
		}

		private void OnLastGameLevelComplete()
		{
			Score = m_currentScore;
		}

		private void OnPowerupUsed(object arg)
		{
			PowerupEvent evt = (PowerupEvent)arg;
			int previousScore= m_currentScore;
			int difference = 0;

			switch (evt.effect) 
			{
			case PowerupEffect.BombCountUp:
				m_currentScore += GlobalConstants.SCORE_FOR_POSITIVE_POWERUP;
				difference = GlobalConstants.SCORE_FOR_POSITIVE_POWERUP;
				break;
			case PowerupEffect.BombCountDown:
				m_currentScore += GlobalConstants.SCORE_FOR_NEGATIVE_POWERUP;
				difference = GlobalConstants.SCORE_FOR_NEGATIVE_POWERUP;
				break;
			case PowerupEffect.BombRangeUp:
				m_currentScore += GlobalConstants.SCORE_FOR_POSITIVE_POWERUP;
				difference = GlobalConstants.SCORE_FOR_POSITIVE_POWERUP;
				break;
			case PowerupEffect.BombRangeDown:
				m_currentScore += GlobalConstants.SCORE_FOR_NEGATIVE_POWERUP;
				difference = GlobalConstants.SCORE_FOR_NEGATIVE_POWERUP;
				break;
			case PowerupEffect.SpeedUp:
				m_currentScore += GlobalConstants.SCORE_FOR_POSITIVE_POWERUP;
				difference = GlobalConstants.SCORE_FOR_POSITIVE_POWERUP;
				break;
			case PowerupEffect.SpeedDown:
				m_currentScore += GlobalConstants.SCORE_FOR_NEGATIVE_POWERUP;
				difference = GlobalConstants.SCORE_FOR_NEGATIVE_POWERUP;
				break;
			}

			if(m_currentScore < 0)
				m_currentScore = 0;

			ScoreEvent scoreEvt = new ScoreEvent(previousScore, m_currentScore, difference);
			NotificationCenter.Dispatch(Notifications.ON_SCORE_CHANGED, scoreEvt, false);
		}

		private void OnEnemyDead()
		{
			ScoreEvent evt = new ScoreEvent(m_currentScore, m_currentScore + GlobalConstants.SCORE_FOR_ENEMY, GlobalConstants.SCORE_FOR_ENEMY);

			m_currentScore += GlobalConstants.SCORE_FOR_ENEMY;
			NotificationCenter.Dispatch(Notifications.ON_SCORE_CHANGED, evt, false);
		}
	}
}