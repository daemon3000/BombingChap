using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class SaveGameLevelIndex : MonoBehaviour 
	{
		private void OnGameLevelComplete()
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.GAME_LEVEL, LevelManager.LoadedLevelIndex + 1);
		}
	}
}