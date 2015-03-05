using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class SaveGameLevelIndex : MonoBehaviour 
	{
		private void Start()
		{
			NotificationCenter.AddObserver(gameObject, Notifications.ON_GAME_LEVEL_COMPLETE);
		}

		private void OnDestroy()
		{
			if(NotificationCenter.Exists)
				NotificationCenter.RemoveObserver(gameObject, Notifications.ON_GAME_LEVEL_COMPLETE);
		}

		private void OnGameLevelComplete()
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.GAME_LEVEL, LevelManager.LoadedLevelIndex + 1);
		}
	}
}