using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class PrefabSet : ScriptableObject
	{
		public GameObject tilemapRenderer;
		public GameObject spPlayer;
		public GameObject spCamera;
		public GameObject mpPlayerOne;
		public GameObject mpPlayerTwo;
		public GameObject mpCameraSystem;
		public GameObject enemy;
		public GameObject flame;
		public GameObject portal;
		public GameObject bombUpPowerup;
		public GameObject bombDownPowerup;
		public GameObject rangeUpPowerup;
		public GameObject rangeDownPowerup;
		public GameObject speedUpPowerup;
		public GameObject speedDownPowerup;

		public GameObject mpOnlineCamera;
		public string mpOnlinePlayerOnePath;
		public string mpOnlinePlayerTwoPath;
		public string mpOnlineBombUpPowerupPath;
		public string mpOnlineBombDownPowerupPath;
		public string mpOnlineRangeUpPowerupPath;
		public string mpOnlineRangeDownPowerupPath;
		public string mpOnlineSpeedUpPowerupPath;
		public string mpOnlineSpeedDownPowerupPath;
	}
}