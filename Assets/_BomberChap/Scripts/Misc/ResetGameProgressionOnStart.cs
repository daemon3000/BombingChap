using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class ResetGameProgressionOnStart : MonoBehaviour
	{
		private void Start()
		{
			int bestScore = PlayerPrefs.GetInt(PlayerPrefsKeys.BEST_SCORE, 0);
			if(SinglePlayerGameController.Score > bestScore)
				PlayerPrefs.SetInt(PlayerPrefsKeys.BEST_SCORE, SinglePlayerGameController.Score);

			PlayerPrefs.SetInt(PlayerPrefsKeys.GAME_LEVEL, -1);
			PlayerPrefs.SetInt(PlayerPrefsKeys.SCORE, 0);
			PlayerPrefs.SetInt(PlayerPrefsKeys.BOMB_COUNT, GlobalConstants.MIN_BOMB_COUNT);
			PlayerPrefs.SetInt(PlayerPrefsKeys.BOMB_RANGE, GlobalConstants.MIN_BOMB_RANGE);
			PlayerPrefs.SetFloat(PlayerPrefsKeys.PLAYER_SPEED, GlobalConstants.MIN_PLAYER_SPEED);
		}
	}
}