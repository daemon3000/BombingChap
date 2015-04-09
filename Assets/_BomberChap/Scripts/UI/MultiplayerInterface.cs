using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	public class MultiplayerInterface : MonoBehaviour 
	{
		[SerializeField]
		private MultiplayerGameController gameController;

		[SerializeField]
		private Text m_playerOneWinsText;
		
		[SerializeField]
		private Text m_playerTwoWinsText;

		[SerializeField]
		private Text m_playerOnePowerupText;

		[SerializeField]
		private Text m_playerTwoPowerupText;

		private Animator m_playerOnePowerupTextAnimator;
		private Animator m_playerTwoPowerupTextAnimator;

		private void Start()
		{
			m_playerOnePowerupTextAnimator = m_playerOnePowerupText.GetComponent<Animator>();
			m_playerTwoPowerupTextAnimator = m_playerTwoPowerupText.GetComponent<Animator>();

			NotificationCenter.AddObserver(gameObject, Notifications.ON_GAME_LEVEL_LOADED);
			NotificationCenter.AddObserver(gameObject, Notifications.ON_POWERUP_USED);
		}
		
		private void OnDestroy()
		{
			if(NotificationCenter.Exists)
			{
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_GAME_LEVEL_LOADED);
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_POWERUP_USED);
			}
		}

		private void OnGameLevelLoaded()
		{
			m_playerOneWinsText.text = "WINS: " + gameController.PlayerOneWins;
			m_playerTwoWinsText.text = "WINS: " + gameController.PlayerTwoWins;
		}
		
		private void OnPowerupUsed(object arg)
		{
			PowerupEvent evt = (PowerupEvent)arg;
			PlayerStats playerStats = evt.target.GetComponent<PlayerStats>();
			Text powerupText = (evt.target.tag == Tags.PlayerOne) ? m_playerOnePowerupText : m_playerTwoPowerupText;
			Animator powerupTextAnimator = (evt.target.tag == Tags.PlayerOne) ? m_playerOnePowerupTextAnimator : m_playerTwoPowerupTextAnimator;

			switch (evt.effect) 
			{
			case PowerupEffect.BombCountUp:
				powerupText.text = string.Format("BOMBS: +1({0})", playerStats.MaxBombs);
				break;
			case PowerupEffect.BombCountDown:
				powerupText.text = string.Format("BOMBS: -1({0})", playerStats.MaxBombs);
				break;
			case PowerupEffect.BombRangeUp:
				powerupText.text = "RANGE: +1";
				break;
			case PowerupEffect.BombRangeDown:
				powerupText.text = "RANGE: -1";
				break;
			case PowerupEffect.SpeedUp:
				powerupText.text = "SPEED: +1";
				break;
			case PowerupEffect.SpeedDown:
				powerupText.text = "SPEED: -1";
				break;
			}
			
			if(powerupTextAnimator != null)
				powerupTextAnimator.SetTrigger("Play");
		}
	}
}