using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(PlayerStats))]
	public class MP_BombManager : BombManagerBase
	{
		[SerializeField]
		private string m_bombPrefabPath;
		[SerializeField]
		private AudioClip m_explosionSound;
		
		private PhotonView m_photonView;
		
		protected override void Start()
		{
			m_photonView = PhotonView.Get(gameObject);
			base.Start();
		}
		
		public override void DropBomb()
		{
			if(!m_photonView.isMine)
				return;

			base.DropBomb();
		}

		protected override GameObject CreateBomb()
		{
			GameObject bomb = PhotonNetwork.Instantiate(m_bombPrefabPath, transform.position, Quaternion.identity, 0);
			bomb.transform.SetParent(transform.parent, true);

			return bomb;
		}
		
		protected override void HandleBombExploded(BombBase bomb)
		{
			Vector3 bombPosition = bomb.transform.position;

			m_activeBombs--;
			bomb.Exploded -= HandleBombExploded;
			DestroyBomb(bomb.gameObject);
			m_photonView.RPC("OnBombExplodedRPC", PhotonTargets.All, bombPosition, m_playerStats.BombRange);
		}

		protected override void DestroyBomb(GameObject bomb)
		{
			PhotonNetwork.Destroy(bomb);
		}

		[PunRPC]
		private void OnBombExplodedRPC(Vector3 position, int bombRange)
		{
			AudioManager.PlaySound(m_explosionSound);
			OnBombExplosion(position, bombRange);
		}

		protected override bool CreatePowerup(Vector3 worldPos)
		{
//			m_photonView.RPC("OnCreatePowerupRPC", PhotonTargets.All, worldPos, Utils.GetRandomEnum<PowerupEffect>());
//			return true;
			return false;
		}

		[PunRPC]
		private void OnCreatePowerupRPC(Vector3 worldPos, PowerupEffect effect)
		{
			if(m_photonView.isMine)
			{
				string prefabPath = GetPowerupPrefabPath(effect);
				if(!string.IsNullOrEmpty(prefabPath))
				{
					GameObject powerupGO = PhotonNetwork.Instantiate(prefabPath, worldPos, Quaternion.identity, 0);
					powerupGO.transform.SetParent(m_currentLevel.transform, true);
				}
				else
				{
					CreateFlame(worldPos);
				}
			}
			else
			{
				if(effect == PowerupEffect.None)
					CreateFlame(worldPos);
			}
		}

		private string GetPowerupPrefabPath(PowerupEffect effect)
		{
			string prefabPath = null;
			switch(effect) 
			{
			case PowerupEffect.BombCountUp:
				prefabPath = m_currentLevel.PrefabSet.mpOnlineBombUpPowerupPath;
				break;
			case PowerupEffect.BombCountDown:
				prefabPath = m_currentLevel.PrefabSet.mpOnlineBombDownPowerupPath;
				break;
			case PowerupEffect.BombRangeUp:
				prefabPath = m_currentLevel.PrefabSet.mpOnlineRangeUpPowerupPath;
				break;
			case PowerupEffect.BombRangeDown:
				prefabPath = m_currentLevel.PrefabSet.mpOnlineRangeDownPowerupPath;
				break;
			case PowerupEffect.SpeedUp:
				prefabPath = m_currentLevel.PrefabSet.mpOnlineSpeedUpPowerupPath;
				break;
			case PowerupEffect.SpeedDown:
				prefabPath = m_currentLevel.PrefabSet.mpOnlineRangeDownPowerupPath;
				break;
			default:
				break;
			}
			
			return prefabPath;
		}
	}
}