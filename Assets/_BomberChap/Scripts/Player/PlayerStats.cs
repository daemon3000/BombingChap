using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class PlayerStats : MonoBehaviour
	{
		private int m_maxBombs = GlobalConstants.MIN_BOMB_COUNT;
		private int m_bombRange = GlobalConstants.MIN_BOMB_RANGE;

		public int MaxBombs
		{
			get { return m_maxBombs; }
			set { m_maxBombs = Mathf.Clamp(value, GlobalConstants.MIN_BOMB_COUNT, GlobalConstants.MAX_BOMB_COUNT); }
		}

		public int BombRange
		{
			get { return m_bombRange; }
			set { m_bombRange = Mathf.Clamp(value, GlobalConstants.MIN_BOMB_RANGE, GlobalConstants.MAX_BOMB_RANGE); }
		}
	}
}