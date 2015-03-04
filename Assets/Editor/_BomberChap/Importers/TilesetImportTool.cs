using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using BomberChap;

namespace BomberChapEditor
{
	public class TilesetImportTool : EditorWindow
	{
		[SerializeField] private Tileset m_tileset;
		[SerializeField] private Material m_tileSheet;
		[SerializeField] private int m_pixelsToUnit;
		[SerializeField] private int m_tileWidth;
		[SerializeField] private int m_tileHeight;
		[SerializeField] private int m_rows;
		[SerializeField] private int m_columns;
		[SerializeField] private int m_border;
		[SerializeField] private int m_spacing;
		[SerializeField] private int m_groundTileID;
		[SerializeField] private int m_solidWallTileID;
		[SerializeField] private int m_destructibleWallTileID;

		private void OnGUI()
		{
			m_tileSheet = EditorGUILayout.ObjectField("Tile Sheet", m_tileSheet, typeof(Material), false) as Material;
			m_pixelsToUnit = Math.Max(EditorGUILayout.IntField("Pixels To Unit", m_pixelsToUnit), 1);
			m_tileWidth = Math.Max(EditorGUILayout.IntField("Tile Width", m_tileWidth), 1);
			m_tileHeight = Math.Max(EditorGUILayout.IntField("Tile Height", m_tileHeight), 1);
			m_rows = Math.Max(EditorGUILayout.IntField("Rows", m_rows), 1);
			m_columns = Math.Max(EditorGUILayout.IntField("Columns", m_columns), 1);
			m_border = Math.Max(EditorGUILayout.IntField("Border", m_border), 0);
			m_spacing = Math.Max(EditorGUILayout.IntField("Spacing", m_spacing), 0);
			m_groundTileID = EditorGUILayout.IntSlider("Ground Tile", m_groundTileID, 0, m_rows * m_columns - 1);
			m_solidWallTileID = EditorGUILayout.IntSlider("Solid Wall Tile", m_solidWallTileID, 0, m_rows * m_columns - 1);
			m_destructibleWallTileID = EditorGUILayout.IntSlider("Destructible Wall Tile", m_destructibleWallTileID, 0, m_rows * m_columns - 1);

			bool wasGUIEnabled = GUI.enabled;
			GUI.enabled = wasGUIEnabled && m_tileSheet != null && m_tileSheet.mainTexture != null;
			if(GUILayout.Button("Import", GUILayout.Height(24.0f)))
				Import();

			GUI.enabled = wasGUIEnabled;
		}

		private void Import()
		{
			if(m_tileset == null)
				CreateTileset();
			if(m_tileset == null)
				return;

			m_tileset.tileSheet = m_tileSheet;
			m_tileset.pixelsToUnit = m_pixelsToUnit;
			m_tileset.tileWidth = m_tileWidth;
			m_tileset.tileHeight = m_tileHeight;
			m_tileset.tiles = new Tile[Tiles.TILE_COUNT];
			for(int tileType = Tiles.START_INDEX; tileType < Tiles.START_INDEX + Tiles.TILE_COUNT; tileType++) 
			{
				if(tileType == Tiles.GROUND)
					m_tileset.tiles[tileType] = CreateTile(m_groundTileID, tileType);
				else if(tileType == Tiles.SOLID_WALL)
					m_tileset.tiles[tileType] = CreateTile(m_solidWallTileID, tileType);
				else
					m_tileset.tiles[tileType] = CreateTile(m_destructibleWallTileID, tileType);
			}

			EditorUtility.DisplayDialog("Success", "The tileset has been sucessfully imported!", "OK");
			Selection.activeObject = m_tileset;
			EditorUtility.SetDirty(m_tileset);
		}

		private void CreateTileset()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save tileset", "tileset", "asset", "");
			if(string.IsNullOrEmpty(path))
				return;

			m_tileset = ScriptableObject.CreateInstance<Tileset>();
			AssetDatabase.CreateAsset(m_tileset, path);
			AssetDatabase.Refresh();
		}

		private Tile CreateTile(int indexInTileSheet, int tileType)
		{
			int tileY = indexInTileSheet / m_columns;
			int tileX = indexInTileSheet - tileY * m_columns;
			Texture tileSheetTex = m_tileSheet.mainTexture;
			Vector2[] uv = new Vector2[4];

			//	UVs for the top left corner of the tile
			uv[0] = new Vector2((m_border + tileX * m_tileWidth + tileX * m_spacing) / (float)tileSheetTex.width,
			                    1.0f - (m_border + tileY * m_tileHeight + tileY * m_spacing) / (float)tileSheetTex.height);
			//	UVs for the top right corner of the tile
			uv[1] = new Vector2((m_border + (tileX + 1) * m_tileWidth + tileX * m_spacing) / (float)tileSheetTex.width,
			                    1.0f - (m_border + tileY * m_tileHeight + tileY * m_spacing) / (float)tileSheetTex.height);
			//	UVs for the bottom right corner of the tile
			uv[2] = new Vector2((m_border + (tileX + 1) * m_tileWidth + tileX * m_spacing) / (float)tileSheetTex.width,
			                    1.0f - (m_border + (tileY + 1) * m_tileHeight + tileY * m_spacing) / (float)tileSheetTex.height);
			//	UVs for the bottom left corner of the tile
			uv[3] = new Vector2((m_border + tileX * m_tileWidth + tileX * m_spacing) / (float)tileSheetTex.width,
			                    1.0f - (m_border + (tileY + 1) * m_tileHeight + tileY * m_spacing) / (float)tileSheetTex.height);

			return Tile.Create(uv, tileType);
		}

		private void SetTileset(Tileset tileset)
		{
			m_tileset = tileset;
			m_tileSheet = tileset.tileSheet;
			m_tileWidth = tileset.tileWidth;
			m_tileHeight = tileset.tileHeight;
		}

		[MenuItem("BomberChap/Create/Tileset")]
		public static void Open()
		{
			EditorWindow.GetWindow<TilesetImportTool>("Import Tileset");
		}

		public static void Open(Tileset tileset)
		{
			var tool = EditorWindow.GetWindow<TilesetImportTool>("Import Tileset");
			tool.SetTileset(tileset);
		}
	}
}