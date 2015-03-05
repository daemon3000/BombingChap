using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class ResetGameProgressionOnStart : MonoBehaviour
	{
		private void Start()
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.GAME_LEVEL, -1);
			PlayerPrefs.SetInt(PlayerPrefsKeys.BOMB_COUNT, GlobalConstants.MIN_BOMB_COUNT);
			PlayerPrefs.SetInt(PlayerPrefsKeys.BOMB_RANGE, GlobalConstants.MIN_BOMB_RANGE);
			PlayerPrefs.SetFloat(PlayerPrefsKeys.PLAYER_SPEED, GlobalConstants.MIN_PLAYER_SPEED);
		}
	}
}