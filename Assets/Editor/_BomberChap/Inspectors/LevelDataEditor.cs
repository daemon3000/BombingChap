using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using BomberChap;

namespace BomberChapEditor
{
	[CustomEditor(typeof(LevelData))]
	public class LevelDataEditor : Editor
	{
		private LevelData m_levelData;

		private void OnEnable()
		{
			m_levelData = target as LevelData;
		}

		public override void OnInspectorGUI()
		{
			m_levelData.source = EditorGUILayout.ObjectField("Source", m_levelData.source, typeof(TextAsset), false) as TextAsset;

			bool wasGUIEnabled = GUI.enabled;
			GUI.enabled = wasGUIEnabled && m_levelData.source != null;
			if(GUILayout.Button("Import", GUILayout.Height(24.0f)))
			{
				if(Import())
					EditorUtility.DisplayDialog("Success", "The level has been successfully imported!", "OK");
				else
					EditorUtility.DisplayDialog("Error", "Failed to import the level!", "OK");
				EditorUtility.SetDirty(target);
			}

			GUI.enabled = wasGUIEnabled;
			if(GUI.changed)
				EditorUtility.SetDirty(target);
		}

		private bool Import()
		{
			List<string> levelLines = new List<string>();
			int levelWidth = 0;

			using(StringReader reader = new StringReader(m_levelData.source.text))
			{
				for(string line; (line = reader.ReadLine()) != null;)
				{
					if(line.Length < LevelData.MIN_LEVEL_WIDTH || line.StartsWith(";") || line.StartsWith("\n")) 
					{
						if(levelLines.Count >= LevelData.MIN_LEVEL_HEIGHT) 
						{
							ParseLevelData(levelLines, levelWidth);
							return true;
						}
						
						levelLines.Clear();
						levelWidth = 0;
						continue;
					}
					
					if(line.Length > levelWidth)
						levelWidth = line.Length;
					levelLines.Add(line);
				}

				if(levelLines.Count >= LevelData.MIN_LEVEL_HEIGHT) 
				{
					ParseLevelData(levelLines, levelWidth);
					return true;
				}
			}

			m_levelData.SetDefault();
			return false;
		}

		private void ParseLevelData(List<string> lines, int width) 
		{
			int levelHeight = lines.Count;
			int[] map = new int[lines.Count * width];
			Vector2 playerPos = new Vector2();
			List<Vector2> enemies = new List<Vector2>();
			
			int count = 0, lineLength = 0, it = 0;
			foreach(string line in lines) 
			{
				it = 0;
				lineLength = line.Length;
				while(it < lineLength && line[it] == ' ') 
				{
					map[count++] = Tiles.NULL;
					it++;
				}
				
				while(it < lineLength) 
				{
					switch(line[it])
					{
					case '@':
						map[count] = Tiles.GROUND;
						playerPos.x = it;
						playerPos.y = count / width;
						break;
					case ' ':
						map[count] = Tiles.GROUND;
						break;
					case '#':
						map[count] = Tiles.SOLID_WALL;
						break;
					case '.':
						map[count] = Tiles.DESTRUCTIBLE_WALL;
						break;
					case 'P':
						map[count] = Tiles.PORTAL;
						break;
					case 'B':
						map[count] = Tiles.BOMB_UP;
						break;
					case 'N':
						map[count] = Tiles.BOMB_DOWN;
						break;
					case 'R':
						map[count] = Tiles.RANGE_UP;
						break;
					case 'T':
						map[count] = Tiles.RANGE_DOWN;
						break;
					case 'S':
						map[count] = Tiles.SPEED_UP;
						break;
					case 'D':
						map[count] = Tiles.SPEED_DOWN;
						break;
					case 'E':
						Vector2 vec = new Vector2();
						vec.x = it;
						vec.y = count / width;
						enemies.Add(vec);
						map[count] = Tiles.GROUND;
						break;
					default:
						map[count] = Tiles.SOLID_WALL;
						break;
					}
					count++;
					it++;
				}
				if(lineLength < width) 
				{
					for(int i = lineLength; i < width; i++)
						map[count++] = Tiles.NULL;
				}
			}

			m_levelData.width = width;
			m_levelData.height = levelHeight;
			m_levelData.map = map;
			m_levelData.playerPosition = playerPos;
			m_levelData.enemyPositions = enemies.ToArray();
		}

		[MenuItem("BomberChap/Create/Level Data")]
		private static void Create()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save level data", "level", "asset", "");
			if(string.IsNullOrEmpty(path))
				return;

			LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
			AssetDatabase.CreateAsset(levelData, path);
			AssetDatabase.Refresh();
			Selection.activeObject = levelData;
		}
	}
}