using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	[Serializable]
	public class Tile
	{
		[SerializeField]
		private Vector2[] m_uv;

		[SerializeField]
		private int m_type;

		public Vector2[] UV 
		{ 
			get { return m_uv; } 
		}

		public int Type 
		{ 
			get { return m_type; } 
		}

		public bool IsSolid
		{
			get { return m_type != Tiles.GROUND; }
		}

		public bool IsDestructible
		{
			get
			{
				return m_type == Tiles.DESTRUCTIBLE_WALL || m_type == Tiles.PORTAL ||
						m_type == Tiles.BOMB_UP || m_type == Tiles.BOMB_DOWN || 
						m_type == Tiles.RANGE_UP || m_type == Tiles.RANGE_DOWN ||
						m_type == Tiles.SPEED_UP || m_type == Tiles.SPEED_DOWN;
			}
		}

		public static Tile Create(Vector2[] uv, int type) 
		{
			Tile tile = new Tile();
			tile.m_uv = uv;
			tile.m_type = type;

			return tile;
		}
	}
}
