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
		private int[] m_map;
		private int m_width;
		private int m_height;
		private int m_allocatedTime;

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

		public int AllocatedTime
		{
			get { return m_allocatedTime; }
		}

		public PrefabSet PrefabSet
		{
			get { return m_prefabSet; }
		}

		private void OnDestroy()
		{
			if(m_tilemapMesh != null)
				m_tilemapMesh.Dispose();
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
			m_allocatedTime = levelData.allocatedTime;
			m_map = (int[])levelData.map.Clone();

			BuildTilemapMesh();
			if(levelData.isMultiPlayerLevel)
			{
				if(levelData.isOnlineMultiPlayerLevel)
				{
#if UNITY_EDITOR
					if(!UnityEditor.EditorApplication.isPlaying)
						return;
#endif
					if(PhotonNetwork.isMasterClient)
					{
						CreateOnlinePlayerAndCamera(prefabSet.mpOnlinePlayerOnePath, prefabSet.mpOnlineCamera, levelData.primaryPlayerPosition);
					}
					else
					{
						CreateOnlinePlayerAndCamera(prefabSet.mpOnlinePlayerTwoPath, prefabSet.mpOnlineCamera, levelData.secondaryPlayerPosition);
					}
				}
				else
				{
					CreateSplitScreenPlayersAndCamera(prefabSet.mpPlayerOne, 
					                                  prefabSet.mpPlayerTwo,
					                                  prefabSet.mpCameraSystem,
					                                  levelData.primaryPlayerPosition,
					                                  levelData.secondaryPlayerPosition);
				}
			}
			else
			{
				CreatePlayerAndCamera(prefabSet.spPlayer, prefabSet.spCamera, levelData.primaryPlayerPosition);
				CreateEnemies(levelData.enemyPositions);
			}
		}

		private void BuildTilemapMesh()
		{
			GameObject tilemapGO = GameObject.Instantiate(m_prefabSet.tilemapRenderer);
			tilemapGO.name = "tilemap";
			tilemapGO.transform.SetParent(transform, false);
			
			MeshFilter meshFilter = tilemapGO.GetComponent<MeshFilter>();
			MeshRenderer meshRenderer = tilemapGO.GetComponent<MeshRenderer>();
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

		private void CreateOnlinePlayerAndCamera(string playerPrefabPath, GameObject cameraPrefab, Vector2 tilePos)
		{
			GameObject playerGO = PhotonNetwork.Instantiate(playerPrefabPath, TileToWorld(tilePos, -1.0f), Quaternion.identity, 0);
			PlayerController playerController = playerGO.GetComponent<PlayerController>();
			playerController.transform.SetParent(transform, true);

			GameObject cameraGO = GameObject.Instantiate(cameraPrefab) as GameObject;
			cameraGO.name = cameraPrefab.name;
			cameraGO.transform.SetParent(transform, false);
			cameraGO.transform.position = new Vector3(playerGO.transform.position.x, playerGO.transform.position.y, -10);
			CameraController camController = cameraGO.GetComponent<CameraController>();
			if(camController != null)
			{
				camController.SetTarget(playerGO.transform);
				playerController.Camera = cameraGO.GetComponent<Camera>();
			}
			else
			{
				Debug.LogWarning("The camera prefab doesn't have a camera controller component");
			}
		}

		private void CreateSplitScreenPlayersAndCamera(GameObject playerOnePrefab, GameObject playerTwoPrefab, GameObject cameraSystem, Vector2 playerOneTilePos, Vector2 playerTwoTilePos)
		{
			GameObject playerOneGO = GameObject.Instantiate(playerOnePrefab) as GameObject;
			playerOneGO.name = playerOnePrefab.name;
			playerOneGO.transform.SetParent(transform, false);
			playerOneGO.transform.position = TileToWorld(playerOneTilePos, -1.0f);
			PlayerController playerOneController = playerOneGO.GetComponent<PlayerController>();

			GameObject playerTwoGO = GameObject.Instantiate(playerTwoPrefab) as GameObject;
			playerTwoGO.name = playerOnePrefab.name;
			playerTwoGO.transform.SetParent(transform, false);
			playerTwoGO.transform.position = TileToWorld(playerTwoTilePos, -1.0f);
			PlayerController playerTwoController = playerTwoGO.GetComponent<PlayerController>();

			GameObject cameraSystemGameObject = GameObject.Instantiate(cameraSystem);
			cameraSystemGameObject.name = cameraSystem.name;
			cameraSystemGameObject.transform.SetParent(transform, false);
			SplitScreenView splitScreenView = cameraSystemGameObject.GetComponent<SplitScreenView>();
			splitScreenView.SetPlayerOne(playerOneGO.transform);
			splitScreenView.SetPlayerTwo(playerTwoGO.transform);
			splitScreenView.Initialize();

			playerOneController.Camera = splitScreenView.CameraOne;
			playerTwoController.Camera = splitScreenView.CameraTwo;
		}

		private void CreatePlayerAndCamera(GameObject playerPrefab, GameObject cameraPrefab, Vector2 tilePos)
		{
			GameObject playerGO = GameObject.Instantiate(playerPrefab) as GameObject;
			playerGO.name = playerPrefab.name;
			playerGO.transform.SetParent(transform, false);
			playerGO.transform.position = TileToWorld(tilePos, -1.0f);
			PlayerController playerController = playerGO.GetComponent<PlayerController>();
			
			GameObject cameraGO = GameObject.Instantiate(cameraPrefab) as GameObject;
			cameraGO.name = cameraPrefab.name;
			cameraGO.transform.SetParent(transform, false);
			cameraGO.transform.position = new Vector3(playerGO.transform.position.x, playerGO.transform.position.y, -10);
			CameraController camController = cameraGO.GetComponent<CameraController>();
			if(camController != null)
			{
				camController.SetTarget(playerGO.transform);
				playerController.Camera = cameraGO.GetComponent<Camera>();
			}
			else
			{
				Debug.LogWarning("The camera prefab doesn't have a camera controller component");
			}
		}

		private void CreateEnemies(Vector2[] positions)
		{
			for(int i = 0; i < positions.Length; i++)
			{
				GameObject enemyGO = GameObject.Instantiate(m_prefabSet.enemy) as GameObject;
				enemyGO.name = m_prefabSet.enemy.name + "_" + i;
				enemyGO.transform.SetParent(transform, false);
				enemyGO.transform.position = TileToWorld(positions[i], -1.0f);
			}
		}

		public void UpdateTilemapMesh()
		{
			m_tilemapMesh.UpdateMesh();
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
	}
}