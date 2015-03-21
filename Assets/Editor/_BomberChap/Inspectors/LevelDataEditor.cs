using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using BomberChap;

namespace BomberChapEditor
{
	[CustomEditor(typeof(LevelData))]
	public class LevelDataEditor : Editor
	{
		private LevelData m_levelData;

		private void OnEnable()
		{
			m_levelData = target as LevelData;
		}

		public override void OnInspectorGUI()
		{
			m_levelData.source = EditorGUILayout.ObjectField("Source", m_levelData.source, typeof(TextAsset), false) as TextAsset;
			m_levelData.allocatedTime = EditorGUILayout.IntField("Allocated Time", m_levelData.allocatedTime);
			m_levelData.allocatedTime = Mathf.Max(m_levelData.allocatedTime, LevelData.MIN_ALLOCATED_TIME);

			bool wasGUIEnabled = GUI.enabled;
			GUI.enabled = wasGUIEnabled && m_levelData.source != null;
			if(GUILayout.Button("Import", GUILayout.Height(24.0f)))
			{
				if(LevelImporter.Import(m_levelData))
					EditorUtility.DisplayDialog("Success", "The level has been successfully imported!", "OK");
				else
					EditorUtility.DisplayDialog("Error", "Failed to import the level!", "OK");
				EditorUtility.SetDirty(target);
			}

			GUI.enabled = wasGUIEnabled;
			if(GUI.changed)
				EditorUtility.SetDirty(target);
		}

		[MenuItem("BomberChap/Create/Level Data")]
		private static void Create()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save level data", "level", "asset", "");
			if(string.IsNullOrEmpty(path))
				return;

			LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
			AssetDatabase.CreateAsset(levelData, path);
			AssetDatabase.Refresh();
			Selection.activeObject = levelData;
		}
	}
}