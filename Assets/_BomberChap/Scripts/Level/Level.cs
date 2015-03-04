using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	[ExecuteInEditMode]
	public class Level : MonoBehaviour
	{
		private TilemapMesh m_tilemapMesh;
		private Tileset m_tileset;
		private PrefabSet m_prefabSet;
		private GameObjectPool m_flamePool;
		private int[] m_map;
		private int m_width;
		private int m_height;

		public int Width
		{
			get { return m_width; }
		}

		public int Height
		{
			get { return m_height; }
		}

		public int TileWidth
		{
			get { return m_tileset != null ? m_tileset.tileWidth : 0; }
		}

		public int TileHeight
		{
			get { return m_tileset != null ? m_tileset.tileHeight : 0; }
		}

		public float PixelToUnit
		{
			get { return m_tileset != null ? 1.0f / m_tileset.pixelsToUnit : 1.0f; }
		}

		private void OnDestroy()
		{
			if(m_tilemapMesh != null)
				m_tilemapMesh.Dispose();
			if(m_flamePool != null)
				m_flamePool.Clear(true);
			m_tilemapMesh = null;
			m_map = null;
			m_tileset = null;
		}

		public void Load(Tileset tileset, PrefabSet prefabSet, LevelData levelData)
		{
			if(tileset == null)
			{
				Debug.LogError("Cannot load level. Tileset is null");
				return;
			}
			if(prefabSet == null)
			{
				Debug.LogError("Cannot load level. Prefab set is null");
				return;
			}
			if(levelData == null)
			{
				Debug.LogError("Cannot load level. Level data is null");
				return;
			}

			m_tileset = tileset;
			m_prefabSet = prefabSet;
			m_width = levelData.width;
			m_height = levelData.height;
			m_map = (int[])levelData.map.Clone();
#if UNITY_EDITOR
			if(UnityEditor.EditorApplication.isPlaying)
				m_flamePool = new GameObjectPool(m_prefabSet.flame);
#else
			m_flamePool = new GameObjectPool(m_prefabSet.flame);
#endif

			BuildTilemapMesh();
			CreatePlayerAndCamera(levelData.playerPosition);
		}

		private void BuildTilemapMesh()
		{
			GameObject tilemapGO = new GameObject("tilemap");
			tilemapGO.transform.SetParent(transform, false);
			
			MeshFilter meshFilter = tilemapGO.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = tilemapGO.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = m_tileset.tileSheet;
			
			Vector3[] vertices = new Vector3[m_map.Length * 4];
			Vector2[] uvs = new Vector2[m_map.Length * 4];
			int[] triangles = new int[m_map.Length * 6];
			int tw = m_tileset.tileWidth, th = m_tileset.tileHeight;
			int r = 0, c = 0;
			int trisCount = 0, vertexCount = 0;
			float pu = 1.0f / m_tileset.pixelsToUnit;
			Tile tile = null;
			
			for(int i = 0; i < m_map.Length; i++)
			{
				r = i / m_width;
				c = i - r * m_width;
				tile = m_tileset.tiles[m_map[i]];
				
				vertices[vertexCount] = new Vector3(c * tw * pu, -(r * th * pu), 0.0f);
				vertices[vertexCount + 1] = new Vector3((c + 1) * tw * pu, -(r * th * pu), 0.0f);
				vertices[vertexCount + 2] = new Vector3((c + 1) * tw * pu, -((r + 1) * th * pu), 0.0f);
				vertices[vertexCount+ 3] = new Vector3(c * tw * pu, -((r + 1) * th * pu), 0.0f);
				
				uvs[vertexCount] = tile.UV[0];
				uvs[vertexCount + 1] = tile.UV[1];
				uvs[vertexCount + 2] = tile.UV[2];
				uvs[vertexCount + 3] = tile.UV[3];
				
				triangles[trisCount] = vertexCount;
				triangles[trisCount + 1] = vertexCount + 1;
				triangles[trisCount + 2] = vertexCount + 3;
				triangles[trisCount + 3] = vertexCount + 3;
				triangles[trisCount + 4] = vertexCount + 1;
				triangles[trisCount + 5] = vertexCount + 2;
				
				vertexCount += 4;
				trisCount += 6;
			}
			
			m_tilemapMesh = new TilemapMesh(meshFilter);
			m_tilemapMesh.vertices = vertices;
			m_tilemapMesh.uvs = uvs;
			m_tilemapMesh.triangles = triangles;
			m_tilemapMesh.UpdateMesh();
		}

		private void CreatePlayerAndCamera(Vector2 tilePos)
		{
			GameObject playerGO = GameObject.Instantiate<GameObject>(m_prefabSet.player);
			playerGO.transform.SetParent(transform, false);
			playerGO.transform.position = TileToWorld(tilePos, -1.0f);

			GameObject cameraGO = GameObject.Instantiate<GameObject>(m_prefabSet.mainCamera);
			cameraGO.transform.SetParent(transform, false);
			cameraGO.transform.position = new Vector3(playerGO.transform.position.x, playerGO.transform.position.y, -10);
			CameraController camController = cameraGO.GetComponent<CameraController>();
			if(camController != null)
				camController.SetTarget(playerGO.transform);
			else
				Debug.LogWarning("The camera prefab doesn't have a camera controller component");
		}

		public void SetAt(int c, int r, int tileType, bool updateMesh)
		{
			SetAt(r * m_width + c, tileType, updateMesh);
		}

		public void SetAt(int index, int tileType, bool updateMesh)
		{
			if(m_map == null || m_tileset == null || index < 0 || index >= m_map.Length || tileType < Tiles.START_INDEX || tileType >= Tiles.TILE_COUNT)
				return;

			m_map[index] = tileType;

			Tile tile = m_tileset.tiles[m_map[index]];
			int startUV = index * 4;
			m_tilemapMesh.uvs[startUV] = tile.UV[0];
			m_tilemapMesh.uvs[startUV + 1] = tile.UV[1];
			m_tilemapMesh.uvs[startUV + 2] = tile.UV[2];
			m_tilemapMesh.uvs[startUV + 3] = tile.UV[3];
			if(updateMesh)
				m_tilemapMesh.UpdateMesh();
		}

		public Tile GetAt(int c, int r)
		{
			return GetAt(r * m_width + c);
		}

		public Tile GetAt(int index)
		{
			if(m_map != null && m_tileset != null && index >= 0 && index < m_map.Length)
				return m_tileset.tiles[m_map[index]];
			else
				return null;
		}

		public Vector2 WorldToTile(Vector3 worldPos)
		{
			Vector2 tilePos = new Vector2(-1, -1);
			if(m_map == null || m_tileset == null)
				return tilePos;

			float worldWidth = m_width * m_tileset.tileWidth * PixelToUnit;
			float worldHeight = m_height * m_tileset.tileHeight * PixelToUnit;
			float worldTileWidth = m_tileset.tileWidth * PixelToUnit;
			float worldTileHeight = m_tileset.tileHeight * PixelToUnit;

			if(worldPos.x < 0.0f || worldPos.x > worldWidth)
				tilePos.x = -1;
			else
				tilePos.x = (int)((worldPos.x + worldTileWidth / 2.0f) / worldTileWidth);

			if(worldPos.y > 0.0f || worldPos.y < -worldHeight)
				tilePos.y = -1;
			else
				tilePos.y = (int)(-(worldPos.y - worldTileHeight / 2.0f) / worldTileHeight);

			return tilePos;
		}

		public Vector3 TileToWorld(Vector2 tilePos, float z)
		{
			return TileToWorld((int)tilePos.x, (int)tilePos.y, z);
		}

		public Vector3 TileToWorld(int c, int r, float z)
		{
			if(m_map == null || m_tileset == null)
				return new Vector3(0, 0, z);
			else
				return new Vector3(c * m_tileset.tileWidth * PixelToUnit, -r * m_tileset.tileHeight * PixelToUnit, z);
		}

		public void OnBombExplosion(Vector3 worldPos, int range)
		{
			Vector2 tilePos = WorldToTile(worldPos);
			int sr = Mathf.Max((int)tilePos.y - range / 2, 0);
			int er = Mathf.Min((int)tilePos.y + range / 2, m_height - 1);
			int sc = Mathf.Max((int)tilePos.x - range / 2, 0);
			int ec = Mathf.Min((int)tilePos.x + range / 2, m_width - 1);
			
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

			m_tilemapMesh.UpdateMesh();
		}

		private bool DestroyTileAndSpawnFlameAt(int c, int r)
		{
			Tile tile = GetAt(c, r);
			if(tile == null)
				return false;

			Vector3 worldPos = TileToWorld(c, r, -1.0f);
			bool continueFlameChain = true;
			if(tile.IsSolid) 
			{
				if(tile.IsDestructible) 
				{
					SetAt(c, r, Tiles.GROUND, false);
					switch(tile.Type) 
					{
					case Tiles.DESTRUCTIBLE_WALL:
						CreateFlame(worldPos);
						break;
					case Tiles.PORTAL:
						CreatePortal(worldPos);
						continueFlameChain = false;
						break;
					case Tiles.BOMB_UP:
						CreatePowerup(m_prefabSet.bombUpPowerup, worldPos);
						continueFlameChain = false;
						break;
					case Tiles.BOMB_DOWN:
						CreatePowerup(m_prefabSet.bombDownPowerup, worldPos);
						continueFlameChain = false;
						break;
					case Tiles.RANGE_UP:
						CreatePowerup(m_prefabSet.rangeUpPowerup, worldPos);
						continueFlameChain = false;
						break;
					case Tiles.RANGE_DOWN:
						CreatePowerup(m_prefabSet.rangeDownPowerup, worldPos);
						continueFlameChain = false;
						break;
					case Tiles.SPEED_UP:
						CreatePowerup(m_prefabSet.speedUpPowerup, worldPos);
						continueFlameChain = false;
						break;
					case Tiles.SPEED_DOWN:
						CreatePowerup(m_prefabSet.speedDownPowerup, worldPos);
						continueFlameChain = false;
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

		private void CreateFlame(Vector3 position)
		{
			GameObject flameGO = m_flamePool.Get();
			flameGO.transform.SetParent(transform, false);
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
			GameObject portalGO = GameObject.Instantiate(m_prefabSet.portal) as GameObject;
			portalGO.transform.SetParent(transform, false);
			portalGO.transform.position = worldPos;
		}

		private void CreatePowerup(GameObject prefab, Vector3 worldPos)
		{
			GameObject powerupGO = GameObject.Instantiate(prefab) as GameObject;
			powerupGO.transform.SetParent(transform, false);
			powerupGO.transform.position = worldPos;
		}
	}
}