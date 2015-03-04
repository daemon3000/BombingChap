using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public static class Tiles
	{
		public const int NULL = -1;
		public const int GROUND = 0;
		public const int SOLID_WALL = 1;
		public const int DESTRUCTIBLE_WALL = 2;
		public const int PORTAL = 3;
		public const int BOMB_UP = 4;
		public const int BOMB_DOWN = 5;
		public const int RANGE_UP = 6;
		public const int RANGE_DOWN = 7;
		public const int SPEED_UP = 8;
		public const int SPEED_DOWN = 9;

		public const int START_INDEX = 0;
		public const int TILE_COUNT = 10;
	}
}