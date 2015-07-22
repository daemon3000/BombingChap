using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class MP_Bomb : BombBase
	{
		[SerializeField]
		private float m_lifeSpan;
		[SerializeField]
		private AudioClip m_fuseSound;
		[SerializeField]
		private Collider2D m_collider;
		
		private AudioSource m_audioSource;
		private PhotonView m_photonView;

		private IEnumerator Start()
		{
			m_photonView = PhotonView.Get(gameObject);
			if(m_photonView.isMine)
			{
				m_collider.enabled = true;
				PlayFuseSound();

				yield return new WaitForSeconds(m_lifeSpan);
				RaiseExplodedEvent();
			}
			else
			{
				m_collider.enabled = false;
				PlayFuseSound();
			}
		}

		private void PlayFuseSound()
		{
			m_audioSource = AudioManager.PlaySound(m_fuseSound);
			if(m_audioSource != null)
				m_audioSource.loop = true;
		}

		private void OnDestroy()
		{
			if(m_audioSource != null)
			{
				m_audioSource.Stop();
				m_audioSource.loop = false;
				m_audioSource = null;
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(m_photonView.isMine && other.tag == Tags.Flame)
			{
				StopAllCoroutines();
				RaiseExplodedEvent();
			}
		}
	}
}