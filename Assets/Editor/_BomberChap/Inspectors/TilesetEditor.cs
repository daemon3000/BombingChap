using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace BomberChapEditor
{
	[CustomEditor(typeof(BomberChap.Tileset))]
	public class TilesetEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			if(GUILayout.Button("Import", GUILayout.Height(24.0f)))
			{
				TilesetImportTool.Open((BomberChap.Tileset)target);
			}
		}
	}
}