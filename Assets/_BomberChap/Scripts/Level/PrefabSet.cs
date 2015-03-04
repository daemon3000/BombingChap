using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class PrefabSet : ScriptableObject
	{
		public GameObject player;
		public GameObject mainCamera;
		public GameObject enemy;
		public GameObject flame;
		public GameObject portal;
		public GameObject bombUpPowerup;
		public GameObject bombDownPowerup;
		public GameObject rangeUpPowerup;
		public GameObject rangeDownPowerup;
		public GameObject speedUpPowerup;
		public GameObject speedDownPowerup;
	}
}