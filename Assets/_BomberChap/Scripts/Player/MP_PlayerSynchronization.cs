using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class MP_PlayerSynchronization : Photon.MonoBehaviour
	{
		[SerializeField]
		private PlayerController m_playerController;
		[SerializeField]
		private MP_BombManager m_bombManager;
		[SerializeField]
		private Collider2D m_collider;

		private void OnAllPlayersLoadedOnlineGameLevel()
		{
			m_playerController.enabled = photonView.isMine;
			m_collider.enabled = photonView.isMine;
			m_bombManager.enabled = true;
		}
	}
}