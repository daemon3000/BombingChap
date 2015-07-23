using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public struct PowerupEvent
	{
		public readonly PowerupEffect effect;
		public readonly string playerTag;
		
		public PowerupEvent(PowerupEffect effect, string playerTag)
		{
			this.effect = effect;
			this.playerTag = playerTag;
		}
	}
}
