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

		[SerializeField]
		private AudioClip m_fuseSound;

		private AudioSource m_audioSource;

		private void OnDestroy()
		{
			ReleaseAudioSource();
		}

		private void ReleaseAudioSource()
		{
			if(m_audioSource != null)
			{
				m_audioSource.Stop();
				m_audioSource.loop = false;
				m_audioSource = null;
			}
		}

		private IEnumerator OnPooledInstanceInitialize()
		{
			m_audioSource = AudioManager.PlaySound(m_fuseSound);
			if(m_audioSource != null)
				m_audioSource.loop = true;

			yield return new WaitForSeconds(m_lifeSpan);
			if(Exploded != null)
				Exploded(this);
		}

		private void OnPooledInstanceReset()
		{
			ReleaseAudioSource();
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