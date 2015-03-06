using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using BomberChap;

namespace BomberChapEditor
{
	public static class LevelImporter
	{
		public static bool Import(LevelData levelData)
		{
			List<string> levelLines = new List<string>();
			int levelWidth = 0;
			
			using(StringReader reader = new StringReader(levelData.source.text))
			{
				for(string line; (line = reader.ReadLine()) != null;)
				{
					if(line.Length < LevelData.MIN_LEVEL_WIDTH || line.StartsWith(";") || line.StartsWith("\n")) 
					{
						if(levelLines.Count >= LevelData.MIN_LEVEL_HEIGHT) 
						{
							ParseLevelData(levelLines, levelWidth, levelData);
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
					ParseLevelData(levelLines, levelWidth, levelData);
					return true;
				}
			}
			
			levelData.SetDefault();
			return false;
		}
		
		private static void ParseLevelData(List<string> lines, int width, LevelData levelData) 
		{
			List<Vector2> enemies = new List<Vector2>();
			int[] map = new int[lines.Count * width];
			Vector2 primaryPlayerPos = Vector2.zero;
			Vector2 secPlayerPos = Vector2.zero;
			int levelHeight = lines.Count;
			bool isMultiPlayerLevel = false;
			
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
						primaryPlayerPos.x = it;
						primaryPlayerPos.y = count / width;
						break;
					case '&':
						map[count] = Tiles.GROUND;
						secPlayerPos.x = it;
						secPlayerPos.y = count / width;
						isMultiPlayerLevel = true;
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
					case 'U':
						map[count] = Tiles.RANDOM_POWERUP;
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
			
			levelData.width = width;
			levelData.height = levelHeight;
			levelData.map = map;
			levelData.primaryPlayerPosition = primaryPlayerPos;
			levelData.secondaryPlayerPosition = secPlayerPos;
			levelData.enemyPositions = enemies.ToArray();
			levelData.isMultiPlayerLevel = isMultiPlayerLevel;
		}
	}
}