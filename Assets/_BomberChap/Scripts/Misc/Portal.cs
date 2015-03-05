using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class Portal : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.tag == Tags.Player)
			{
				if(!LevelManager.IsLastLevelLoaded)
				{
					NotificationCenter.Dispatch(Notifications.ON_GAME_LEVEL_COMPLETE);
					LevelManager.LoadLevel(LevelManager.LoadedLevelIndex + 1);
				}
				else
				{
					NotificationCenter.Dispatch(Notifications.ON_LAST_GAME_LEVEL_COMPLETE);
				}
			}
		}
	}
}