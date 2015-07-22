using UnityEngine;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(PlayerStats))]
	public class BombManager : BombManagerBase
	{
		[SerializeField]
		private GameObject m_bombPrefab;
		[SerializeField]
		private AudioClip m_explosionSound;

		private GameObjectPool m_bombPool;

		protected override void Start()
		{
			m_bombPool = new GameObjectPool(m_bombPrefab);
			base.Start();
		}

		protected override void OnDestroy()
		{
			if(m_bombPool != null)
				m_bombPool.Clear(true);
			base.OnDestroy();
		}

		protected override GameObject CreateBomb()
		{
			GameObject bomb = m_bombPool.Get();
			bomb.transform.SetParent(transform.parent, false);
			bomb.transform.position = transform.position;
			return bomb;
		}

		protected override void HandleBombExploded(BombBase bomb)
		{
			AudioManager.PlaySound(m_explosionSound);
			base.HandleBombExploded(bomb);
		}

		protected override void DestroyBomb(GameObject bomb)
		{
			m_bombPool.Free(bomb);
		}
	}
}