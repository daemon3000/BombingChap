using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class ResetLevelOnPlayerDeath : MonoBehaviour 
	{
		[SerializeField]
		private float m_delay;

		private IEnumerator OnTriggerEnter2D(Collider2D other)
		{
			if(this.tag == Tags.Player && (other.tag == Tags.Flame || other.tag == Tags.Enemy))
			{
				yield return new WaitForSeconds(m_delay);
				LevelManager.ReloadCurrentLevel();
			}
		}
	}
}