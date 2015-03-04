using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using BomberChap;

namespace BomberChapEditor
{
	public static class PrefabSetTool
	{
		[MenuItem("BomberChap/Create/Prefab Set")]
		private static void Create()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save prefab set", "prefab_set", "asset", "");
			if(string.IsNullOrEmpty(path))
				return;
			
			PrefabSet prefabSet = ScriptableObject.CreateInstance<PrefabSet>();
			AssetDatabase.CreateAsset(prefabSet, path);
			AssetDatabase.Refresh();
			Selection.activeObject = prefabSet;
		}
	}
}