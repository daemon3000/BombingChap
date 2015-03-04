using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class Tileset : ScriptableObject
	{
		public Material tileSheet;
		public Tile[] tiles;
		public int tileWidth;
		public int tileHeight;
		public int pixelsToUnit;
	}
}