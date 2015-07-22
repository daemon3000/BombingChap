using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class BombBase : MonoBehaviour 
	{
		public event Action<BombBase> Exploded;

		protected void RaiseExplodedEvent()
		{
			if(Exploded != null)
				Exploded(this);
		}
	}
}