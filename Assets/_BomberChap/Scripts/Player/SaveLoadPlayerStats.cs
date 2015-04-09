using UnityEngine;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(CharacterMotor))]
	[RequireComponent(typeof(PlayerStats))]
	public class SaveLoadPlayerStats : MonoBehaviour 
	{
		private void Start()
		{
			PlayerStats playerStats = GetComponent<PlayerStats>();
			CharacterMotor motor = GetComponent<CharacterMotor>();

			playerStats.MaxBombs = PlayerPrefs.GetInt(PlayerPrefsKeys.SP_BOMB_COUNT, GlobalConstants.MIN_BOMB_COUNT);
			playerStats.BombRange = PlayerPrefs.GetInt(PlayerPrefsKeys.SP_BOMB_RANGE, GlobalConstants.MIN_BOMB_RANGE);
			motor.Speed = PlayerPrefs.GetFloat(PlayerPrefsKeys.SP_PLAYER_SPEED, GlobalConstants.MIN_PLAYER_SPEED);
		}

		private void OnGameLevelComplete()
		{
			PlayerStats playerStats = GetComponent<PlayerStats>();
			CharacterMotor motor = GetComponent<CharacterMotor>();

			PlayerPrefs.SetInt(PlayerPrefsKeys.SP_BOMB_COUNT, playerStats.MaxBombs);
			PlayerPrefs.SetInt(PlayerPrefsKeys.SP_BOMB_RANGE, playerStats.BombRange);
			PlayerPrefs.SetFloat(PlayerPrefsKeys.SP_PLAYER_SPEED, motor.Speed);
		}
	}
}