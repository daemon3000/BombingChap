using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public abstract class MultiplayerGameControllerBase : MonoBehaviour 
	{
		public abstract int MaxRounds { get; }
		public abstract int PlayerOneWins { get; }
		public abstract int PlayerTwoWins { get; }
	}
}
