﻿using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class LevelData : ScriptableObject
	{
		public const int MAX_LEVEL_WIDTH = 120;
		public const int MIN_LEVEL_WIDTH = 4;
		public const int MAX_LEVEL_HEIGHT = 120;
		public const int MIN_LEVEL_HEIGHT = 4;
		public const int MIN_ALLOCATED_TIME = 20;

		public TextAsset source;
		public int allocatedTime;
		public int width;
		public int height;
		public int[] map;
		public Vector2 primaryPlayerPosition;
		public Vector2 secondaryPlayerPosition;
		public Vector2[] enemyPositions;
		public bool isMultiPlayerLevel;
		public bool isOnlineMultiPlayerLevel;

		public void SetDefault()
		{
			width = MIN_LEVEL_WIDTH;
			height = MIN_LEVEL_HEIGHT;
			map = new int[width * height];
			for(int i = 0; i < map.Length; i++)
				map[i] = Tiles.GROUND;
		}
	}
}