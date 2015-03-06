using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class ResetLevelOnPlayerDeath : MonoBehaviour 
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			if(this.tag == Tags.Player && (other.tag == Tags.Flame || other.tag == Tags.Enemy))
				LevelManager.ReloadCurrentLevel();
		}
	}
}