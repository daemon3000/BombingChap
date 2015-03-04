using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections;
using BomberChap;

namespace BomberChapEditor
{
	[CustomEditor(typeof(LevelManager))]
	public class LevelManagerEditor : Editor
	{
		private SerializedProperty m_tileset;
		private SerializedProperty m_prefabSet;
		private SerializedProperty m_screenFader;
		private SerializedProperty m_loadLevelOnStart;
		private ReorderableList m_levels;
		private LevelManager m_levelManager;

		private void OnEnable()
		{
			m_tileset = serializedObject.FindProperty("m_tileset");
			m_prefabSet = serializedObject.FindProperty("m_prefabSet");
			m_screenFader = serializedObject.FindProperty("m_screenFader");
			m_loadLevelOnStart = serializedObject.FindProperty("m_loadLevelOnStart");
			m_levels = new ReorderableList(serializedObject, serializedObject.FindProperty("m_levels"), true, true, true, true);
			m_levels.drawElementCallback += HandleDrawLevels;
			m_levels.drawHeaderCallback += HandleDrawLevelHeader;
			m_levelManager = target as LevelManager;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(m_tileset);
			EditorGUILayout.PropertyField(m_prefabSet);
			EditorGUILayout.PropertyField(m_screenFader);
			EditorGUILayout.PropertyField(m_loadLevelOnStart);
			EditorGUILayout.Space();
			m_levels.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}

		private void HandleDrawLevels(Rect rect, int index, bool isActive, bool isFocused)
		{
			Rect levelRect = new Rect(rect.x, rect.y, rect.width - 135.0f, rect.height);
			Rect previewButtonRect = new Rect(levelRect.xMax + 5.0f, rect.y, 60.0f, rect.height);
			Rect closeButtonRect = new Rect(previewButtonRect.xMax + 5.0f, rect.y, 60.0f, rect.height);
			var level = m_levels.serializedProperty.GetArrayElementAtIndex(index);

			EditorGUI.PropertyField(levelRect, level, GUIContent.none);
			GUI.enabled = index != m_levelManager.PreviewLevelIndex;
			if(GUI.Button(previewButtonRect, "Preview"))
			{
				m_levelManager.PreviewLevel(index);
			}
			GUI.enabled = index == m_levelManager.PreviewLevelIndex;
			if(GUI.Button(closeButtonRect, "Close"))
			{
				m_levelManager.DestroyPreviewLevel();
			}
			GUI.enabled = true;
		}

		private void HandleDrawLevelHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Levels");
		}
	}
}