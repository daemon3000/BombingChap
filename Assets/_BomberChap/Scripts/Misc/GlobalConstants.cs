using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public static class GlobalConstants
	{
		public const float MAX_PLAYER_SPEED = 5.0f;
		public const float MIN_PLAYER_SPEED = 2.0f;
		public const float PLAYER_SPEED_INCREMENT = MIN_PLAYER_SPEED * 0.35f;
		public const int MAX_BOMB_RANGE = 15;
		public const int MIN_BOMB_RANGE = 3;
		public const int BOMB_RANGE_INCREMENT = 2;
		public const int MAX_BOMB_COUNT = 5;
		public const int MIN_BOMB_COUNT = 1;
		public const int BOMB_COUNT_INCREMENT = 1;
		public const int SCORE_FOR_POSITIVE_POWERUP = 100;
		public const int SCORE_FOR_NEGATIVE_POWERUP = -50;
		public const int SCORE_FOR_ENEMY = 100;
	}
}