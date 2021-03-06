﻿using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class MP_EndRoundOnPlayerDeath : MonoBehaviour 
	{
		private IEnumerator OnTriggerEnter2D(Collider2D other)
		{
			if((tag == Tags.PlayerOne || tag == Tags.PlayerTwo) && (other.tag == Tags.Flame || other.tag == Tags.Enemy))
			{
				yield return null;
				NotificationCenter.Dispatch(Notifications.ON_PLAYER_DEAD);
			}
		}
	}
}