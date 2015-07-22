using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public abstract class BombManagerBase : MonoBehaviour 
	{
		protected PlayerStats m_playerStats;
		protected Level m_currentLevel;
		protected int m_activeBombs;
		private GameObjectPool m_flamePool;

		protected virtual void Start()
		{
			m_playerStats = GetComponent<PlayerStats>();
			m_currentLevel = LevelManager.GetLoadedLevel();
			m_flamePool = new GameObjectPool(m_currentLevel.PrefabSet.flame);
			m_activeBombs = 0;
		}

		protected virtual void OnDestroy()
		{
			if(m_flamePool != null)
				m_flamePool.Clear(true);
		}
		
		public virtual void DropBomb()
		{
			if(m_activeBombs < m_playerStats.MaxBombs)
			{
				GameObject bombGO = CreateBomb();
				BombBase bomb = bombGO.GetComponent<BombBase>();
				bomb.Exploded += HandleBombExploded;
				m_activeBombs++;
			}
		}

		protected abstract GameObject CreateBomb();
		
		protected virtual void HandleBombExploded(BombBase bomb)
		{
			Vector3 bombPosition = bomb.transform.position;

			m_activeBombs--;
			bomb.Exploded -= HandleBombExploded;
			DestroyBomb(bomb.gameObject);
			OnBombExplosion(bombPosition, m_playerStats.BombRange);
		}

		protected abstract void DestroyBomb(GameObject bomb);

		protected void OnBombExplosion(Vector3 worldPos, int range)
		{
			Vector2 tilePos = m_currentLevel.WorldToTile(worldPos);
			int sr = Mathf.Max((int)tilePos.y - range / 2, 0);
			int er = Mathf.Min((int)tilePos.y + range / 2, m_currentLevel.Height - 1);
			int sc = Mathf.Max((int)tilePos.x - range / 2, 0);
			int ec = Mathf.Min((int)tilePos.x + range / 2, m_currentLevel.Width - 1);
			
			DestroyTileAndSpawnFlameAt((int)tilePos.x, (int)tilePos.y);
			for(int r = (int)tilePos.y - 1; r >= sr; r--) {
				if(!DestroyTileAndSpawnFlameAt((int)tilePos.x, r))
					break;
			}
			for(int r = (int)tilePos.y + 1; r <= er; r++) {
				if(!DestroyTileAndSpawnFlameAt((int)tilePos.x, r))
					break;
			}
			for(int c = (int)tilePos.x - 1; c >= sc; c--) {
				if(!DestroyTileAndSpawnFlameAt(c, (int)tilePos.y))
					break;
			}
			for(int c = (int)tilePos.x + 1; c <= ec; c++) {
				if(!DestroyTileAndSpawnFlameAt(c, (int)tilePos.y))
					break;
			}

			m_currentLevel.UpdateTilemapMesh();
		}
		
		private bool DestroyTileAndSpawnFlameAt(int c, int r)
		{
			Tile tile = m_currentLevel.GetAt(c, r);
			if(tile == null)
				return false;
			
			Vector3 worldPos = m_currentLevel.TileToWorld(c, r, -1.0f);
			bool continueFlameChain = true;
			if(tile.IsSolid) 
			{
				if(tile.IsDestructible) 
				{
					m_currentLevel.SetAt(c, r, Tiles.GROUND, false);
					switch(tile.Type) 
					{
					case Tiles.DESTRUCTIBLE_WALL:
						CreateFlame(worldPos);
						break;
					case Tiles.PORTAL:
						CreatePortal(worldPos);
						break;
					case Tiles.RANDOM_POWERUP:
						if(!CreatePowerup(worldPos))
							CreateFlame(worldPos);
						break;
					default:
						CreateFlame(worldPos);
						break;
					}
				}
				else 
				{
					continueFlameChain = false;
				}
			}
			else 
			{
				CreateFlame(worldPos);
			}
			
			return continueFlameChain;
		}
		
		protected void CreateFlame(Vector3 position)
		{
			GameObject flameGO = m_flamePool.Get();
			flameGO.transform.SetParent(m_currentLevel.transform, false);
			flameGO.transform.position = position;
			
			Flame flame = flameGO.GetComponent<Flame>();
			flame.Extinguished += HandleFlameExtinguished;
		}
		
		private void HandleFlameExtinguished(Flame flame)
		{
			flame.Extinguished -= HandleFlameExtinguished;
			m_flamePool.Free(flame.gameObject);
		}
		
		private void CreatePortal(Vector3 worldPos)
		{
			GameObject portalGO = GameObject.Instantiate(m_currentLevel.PrefabSet.portal) as GameObject;
			portalGO.transform.SetParent(m_currentLevel.transform, false);
			portalGO.transform.position = worldPos;
		}
		
		protected virtual bool CreatePowerup(Vector3 worldPos)
		{
			GameObject prefab = GetRandomPowerupPrefab();
			if(prefab != null)
			{
				GameObject powerupGO = GameObject.Instantiate(prefab) as GameObject;
				powerupGO.transform.SetParent(m_currentLevel.transform, false);
				powerupGO.transform.position = worldPos;
			}
			
			return prefab != null;
		}
		
		protected GameObject GetRandomPowerupPrefab()
		{
			PowerupEffect effect = Utils.GetRandomEnum<PowerupEffect>();
			return GetPowerupPrefab(effect);
		}

		protected GameObject GetPowerupPrefab(PowerupEffect effect)
		{
			GameObject prefab = null;
			switch(effect) 
			{
			case PowerupEffect.BombCountUp:
				prefab = m_currentLevel.PrefabSet.bombUpPowerup;
				break;
			case PowerupEffect.BombCountDown:
				prefab = m_currentLevel.PrefabSet.bombDownPowerup;
				break;
			case PowerupEffect.BombRangeUp:
				prefab = m_currentLevel.PrefabSet.rangeUpPowerup;
				break;
			case PowerupEffect.BombRangeDown:
				prefab = m_currentLevel.PrefabSet.rangeDownPowerup;
				break;
			case PowerupEffect.SpeedUp:
				prefab = m_currentLevel.PrefabSet.speedUpPowerup;
				break;
			case PowerupEffect.SpeedDown:
				prefab = m_currentLevel.PrefabSet.speedDownPowerup;
				break;
			default:
				break;
			}
			
			return prefab;
		}
	}
}