using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class MP_PlayerSynchronization : Photon.PunBehaviour
	{
		[SerializeField]
		private PlayerController m_playerController;
		[SerializeField]
		private MP_BombManager m_bombManager;
		[SerializeField]
		private Collider2D m_collider;
		[SerializeField]
		private CharacterMotor m_motor;
		[SerializeField]
		private Animator m_animator;
		[SerializeField]
		private PlayerAnimatorParameters m_animatorParameters;

		private void OnAllPlayersLoadedOnlineGameLevel()
		{
			m_playerController.enabled = photonView.isMine;
			m_collider.enabled = photonView.isMine;
			m_bombManager.enabled = true;
		}

		private void OnPlayerChangedDestination(object arg)
		{
			object[] param = (object[])arg;
			photonView.RPC("OnPlayerChangedDestinationRPC", PhotonTargets.Others, 
			               (Vector3)param[0], (int)param[1], (int)param[2], (int)param[3], (int)param[4]);
		}

		[PunRPC]
		private void OnPlayerChangedDestinationRPC(Vector3 destination, int hDir, int vDir, int lastHDir, int lastVDir)
		{
			if(photonView.isMine)
				return;

			m_motor.SetDestination(destination);
			if(vDir > 0) 
			{
				if(vDir != lastVDir)
					m_animator.SetTrigger(m_animatorParameters.StartMoveUp);
			}
			else if(vDir < 0) 
			{
				if(vDir != lastVDir)
					m_animator.SetTrigger(m_animatorParameters.StartMoveDown);
			}
			else if(hDir > 0) 
			{
				if(hDir != lastHDir)
					m_animator.SetTrigger(m_animatorParameters.StartMoveRight);
			}
			else if(hDir < 0) 
			{
				if(hDir != lastHDir)
					m_animator.SetTrigger(m_animatorParameters.StartMoveLeft);
			}

			m_animator.SetInteger(m_animatorParameters.MoveHorizontal, hDir);
			m_animator.SetInteger(m_animatorParameters.MoveVertical, vDir);
		}

		private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if(stream.isWriting)
			{
				stream.SendNext(m_animator.GetInteger(m_animatorParameters.MoveHorizontal));
				stream.SendNext(m_animator.GetInteger(m_animatorParameters.MoveVertical));
			}
			else
			{
				m_animator.SetInteger(m_animatorParameters.MoveHorizontal, (int)stream.ReceiveNext());
				m_animator.SetInteger(m_animatorParameters.MoveVertical, (int)stream.ReceiveNext());
			}
		}
	}
}