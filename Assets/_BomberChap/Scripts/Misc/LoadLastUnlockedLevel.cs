using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class LoadLastUnlockedLevel : MonoBehaviour 
	{
		private void Start()
		{
			int gameLevel = Mathf.Max(PlayerPrefs.GetInt(PlayerPrefsKeys.SP_GAME_LEVEL, 0), 0);
			LevelManager.LoadLevel(gameLevel, false);
		}
	}
}