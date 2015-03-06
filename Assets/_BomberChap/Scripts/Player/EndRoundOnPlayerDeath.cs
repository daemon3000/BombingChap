using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class EndRoundOnPlayerDeath : MonoBehaviour 
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			if((this.tag == Tags.PlayerOne || this.tag == Tags.PlayerTwo) && (other.tag == Tags.Flame || other.tag == Tags.Enemy))
			{
				if(this.tag == Tags.PlayerOne)
					NotificationCenter.Dispatch(Notifications.ON_PLAYER_ONE_DEAD);
				else
					NotificationCenter.Dispatch(Notifications.ON_PLAYER_TWO_DEAD);
			}
		}
	}
}