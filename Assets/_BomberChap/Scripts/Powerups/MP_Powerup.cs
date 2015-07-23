using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class MP_Powerup : Photon.MonoBehaviour 
	{
		[SerializeField]
		private PowerupEffect m_effect;
		[SerializeField]
		private AudioClip m_effectSound;
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.tag == Tags.PlayerOne || other.tag == Tags.PlayerTwo)
			{
				photonView.RPC("OnCollidedWithPlayerRPC", PhotonTargets.All, other.tag);
			}
			else if(other.tag == Tags.Flame || other.tag == Tags.Enemy)
			{
				photonView.RPC("OnCollidedWithFlameOrEnemyRPC", PhotonTargets.All);
			}
		}
		
		private void ApplyEffect(GameObject playerGO)
		{
			PlayerStats playerStats = playerGO.GetComponent<PlayerStats>();
			CharacterMotor motor = playerGO.GetComponent<CharacterMotor>();
			if(playerStats == null || motor == null)
			{
				Debug.LogWarning("Failed to apply powerup effect. The player is missing some required components");
				return;
			}
			
			switch (m_effect) 
			{
			case PowerupEffect.BombCountUp:
				playerStats.MaxBombs += GlobalConstants.BOMB_COUNT_INCREMENT;
				break;
			case PowerupEffect.BombCountDown:
				playerStats.MaxBombs -= GlobalConstants.BOMB_COUNT_INCREMENT;
				break;
			case PowerupEffect.BombRangeUp:
				playerStats.BombRange += GlobalConstants.BOMB_RANGE_INCREMENT;
				break;
			case PowerupEffect.BombRangeDown:
				playerStats.BombRange -= GlobalConstants.BOMB_RANGE_INCREMENT;
				break;
			case PowerupEffect.SpeedUp:
				motor.Speed += GlobalConstants.PLAYER_SPEED_INCREMENT;
				if(motor.Speed > GlobalConstants.MAX_PLAYER_SPEED)
					motor.Speed = GlobalConstants.MAX_PLAYER_SPEED;
				break;
			case PowerupEffect.SpeedDown:
				motor.Speed -= GlobalConstants.PLAYER_SPEED_INCREMENT;
				if(motor.Speed < GlobalConstants.MIN_PLAYER_SPEED)
					motor.Speed = GlobalConstants.MIN_PLAYER_SPEED;
				break;
			default:
				break;
			}
		}

		[PunRPC]
		private void OnCollidedWithPlayerRPC(string playerTag)
		{
			GameObject playerGO = GameObject.FindGameObjectWithTag(playerTag);

			ApplyEffect(playerGO);
			AudioManager.PlaySound(m_effectSound);
			NotificationCenter.Dispatch(Notifications.ON_POWERUP_USED, new PowerupEvent(m_effect, playerTag), false);

			if(photonView.isMine)
				PhotonNetwork.Destroy(gameObject);
		}

		[PunRPC]
		private void OnCollidedWithFlameOrEnemyRPC()
		{
			if(photonView.isMine)
				PhotonNetwork.Destroy(gameObject);
		}
	}
}