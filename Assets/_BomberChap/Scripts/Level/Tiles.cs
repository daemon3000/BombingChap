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
		public const int RANDOM_POWERUP = 4;

		public const int START_INDEX = 0;
		public const int TILE_COUNT = 5;
	}
}