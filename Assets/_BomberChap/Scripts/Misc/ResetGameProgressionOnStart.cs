using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class ResetGameProgressionOnStart : MonoBehaviour
	{
		private void Start()
		{
			int bestScore = PlayerPrefs.GetInt(PlayerPrefsKeys.SP_BEST_SCORE, 0);
			if(SinglePlayerGameController.Score > bestScore)
				PlayerPrefs.SetInt(PlayerPrefsKeys.SP_BEST_SCORE, SinglePlayerGameController.Score);

			PlayerPrefs.SetInt(PlayerPrefsKeys.SP_GAME_LEVEL, -1);
			PlayerPrefs.SetInt(PlayerPrefsKeys.SP_SCORE, 0);
			PlayerPrefs.SetInt(PlayerPrefsKeys.SP_BOMB_COUNT, GlobalConstants.MIN_BOMB_COUNT);
			PlayerPrefs.SetInt(PlayerPrefsKeys.SP_BOMB_RANGE, GlobalConstants.MIN_BOMB_RANGE);
			PlayerPrefs.SetFloat(PlayerPrefsKeys.SP_PLAYER_SPEED, GlobalConstants.MIN_PLAYER_SPEED);
		}
	}
}