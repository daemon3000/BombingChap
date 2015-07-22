using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public static class Notifications
	{
		public const string ON_LAST_GAME_LEVEL_COMPLETE = "OnLastGameLevelComplete";
		public const string ON_GAME_LEVEL_COMPLETE = "OnGameLevelComplete";
		public const string ON_PLAYER_DEAD = "OnPlayerDead";
		public const string ON_PLAYER_ONE_DEAD = "OnPlayerOneDead";
		public const string ON_PLAYER_TWO_DEAD = "OnPlayerTwoDead";
		public const string ON_MULTI_PLAYER_MATCH_OVER = "OnMultiPlayerMatchOver";
		public const string ON_GAME_LEVEL_WILL_UNLOAD = "OnGameLevelWillUnload";
		public const string ON_GAME_LEVEL_LOADED = "OnGameLevelLoaded";
		public const string ON_ALL_PLAYERS_LOADED_ONLINE_GAME_LEVEL = "OnAllPlayersLoadedOnlineGameLevel";
		public const string ON_POWERUP_USED = "OnPowerupUsed";
		public const string ON_SCORE_CHANGED = "OnScoreChanged";
		public const string ON_ENEMY_DEAD = "OnEnemyDead";
		public const string ON_GAME_PAUSED = "OnGamePaused";
		public const string ON_GAME_UNPAUSED = "OnGameUnpaused";
	}
}