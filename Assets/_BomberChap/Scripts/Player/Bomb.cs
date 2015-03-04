using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class Bomb : MonoBehaviour 
	{
		public event Action<Bomb> Exploded;

		[SerializeField]
		private float m_lifeSpan;

		private IEnumerator OnPooledInstanceInitialize()
		{
			yield return new WaitForSeconds(m_lifeSpan);
			if(Exploded != null)
				Exploded(this);
		}

		private void OnPooledInstanceReset()
		{
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.tag == Tags.Flame)
			{
				StopAllCoroutines();
				if(Exploded != null)
					Exploded(this);
			}
		}
	}
}