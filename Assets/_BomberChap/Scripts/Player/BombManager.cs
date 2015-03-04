using UnityEngine;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(PlayerStats))]
	public class BombManager : MonoBehaviour 
	{
		[SerializeField]
		private GameObject m_bombPrefab;

		private GameObjectPool m_bombPool;
		private PlayerStats m_playerStats;
		private Level m_currentLevel;
		private int m_activeBombs;

		private void Start()
		{
			m_bombPool = new GameObjectPool(m_bombPrefab);
			m_playerStats = GetComponent<PlayerStats>();
			m_currentLevel = LevelManager.GetLoadedLevel();
			m_activeBombs = 0;
		}

		public void DropBomb()
		{
			if(m_activeBombs < m_playerStats.MaxBombs)
			{
				GameObject bombGO = m_bombPool.Get();
				bombGO.transform.SetParent(transform.parent, false);
				bombGO.transform.position = transform.position;

				Bomb bomb = bombGO.GetComponent<Bomb>();
				bomb.Exploded += HandleBombExploded;
				m_activeBombs++;
			}
		}

		private void HandleBombExploded(Bomb bomb)
		{
			bomb.Exploded -= HandleBombExploded;
			m_currentLevel.OnBombExplosion(bomb.transform.position, m_playerStats.BombRange);
			m_bombPool.Free(bomb.gameObject);
			m_activeBombs--;
		}

		private void OnDestroy()
		{
			m_bombPool.Clear(true);
		}
	}
}