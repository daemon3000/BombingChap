using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class Flame : MonoBehaviour 
	{
		public event Action<Flame> Extinguished;
		
		[SerializeField]
		private float m_lifeSpan;
		
		private IEnumerator OnPooledInstanceInitialize()
		{
			yield return new WaitForSeconds(m_lifeSpan);
			if(Extinguished != null)
				Extinguished(this);
		}
		
		private void OnPooledInstanceReset()
		{
		}
	}
}