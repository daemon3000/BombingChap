using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class LevelManager : MonoBehaviour
	{
		[SerializeField]
		private Tileset m_tileset;
		[SerializeField]
		private PrefabSet m_prefabSet;
		[SerializeField]
		private ScreenFader m_screenFader;
		[SerializeField]
		private bool m_loadLevelOnStart;
		[SerializeField]
		private LevelData[] m_levels;

		private Level m_loadedLevel = null;
		private int m_loadedLevelIndex = -1;
		private static LevelManager m_instance;

#if UNITY_EDITOR
		private Level m_editorPreviewLevel = null;
		private int m_editorPreviewLevelIndex = -1;
#endif

		private void Start()
		{
			if(m_instance != null)
				Debug.LogWarning("You shouldn't have more than one level manager in the scene");

			m_instance = this;
			if(m_loadLevelOnStart && m_levels.Length > 0)
				LoadLevelInternal(0);
		}

		private void LoadLevelInternal(int index)
		{
			UnloadCurrentLevel();
			if(index >= 0 && index < m_levels.Length)
			{
				GameObject levelGO = new GameObject("__LEVEL_ROOT");
				m_loadedLevel = levelGO.AddComponent<Level>();
				m_loadedLevel.Load(m_tileset, m_prefabSet, m_levels[index]);
				m_loadedLevelIndex = index;
				NotificationCenter.Dispatch(Notifications.ON_GAME_LEVEL_LOADED);
			}
			else
			{
				Debug.LogError("Level index is out of range");
			}
		}

		private IEnumerator LoadLevelWithFade(int index)
		{
			PauseManager.Pause();
			if(m_screenFader != null)
				yield return m_screenFader.FadeOut();
			else
				yield return null;
			
			LoadLevelInternal(index);
			
			if(m_screenFader != null)
				yield return m_screenFader.FadeIn();
			else
				yield return null;
			
			PauseManager.UnPause();
			m_screenFader.ClearFade();
		}

		private void UnloadCurrentLevel()
		{
			if(m_loadedLevel != null)
			{
				NotificationCenter.Dispatch(Notifications.ON_GAME_LEVEL_WILL_UNLOAD);
				GameObject.Destroy(m_loadedLevel.gameObject);
				m_loadedLevelIndex = -1;
			}
		}

		public static int LevelCount
		{
			get { return m_instance.m_levels.Length; }
		}

		public static int LoadedLevelIndex
		{
			get { return m_instance.m_loadedLevelIndex; }
		}

		public static bool IsLastLevelLoaded
		{
			get { return m_instance.m_loadedLevelIndex == (m_instance.m_levels.Length - 1); }
		}

		public static Level GetLoadedLevel()
		{
			return m_instance.m_loadedLevel;
		}

		public static void LoadLevel(int index, bool fade = true)
		{
			if(fade)
				m_instance.StartCoroutine(m_instance.LoadLevelWithFade(index));
			else
				m_instance.LoadLevelInternal(index);
		}

		public static void ReloadCurrentLevel(bool fade = true)
		{
			if(LoadedLevelIndex >= 0)
				LoadLevel(LoadedLevelIndex, fade);
			else
				Debug.LogError("There is no level currently loaded");
		}

#if UNITY_EDITOR
		public int PreviewLevelIndex
		{
			get { return m_editorPreviewLevelIndex; }
		}

		public void PreviewLevel(int index)
		{
			DestroyPreviewLevel();
			if(index >= 0 && index < m_levels.Length)
			{
				GameObject levelGO = new GameObject("__EDITOR_LEVEL_ROOT");
				m_editorPreviewLevel = levelGO.AddComponent<Level>();
				m_editorPreviewLevel.Load(m_tileset, m_prefabSet, m_levels[index]);
				m_editorPreviewLevelIndex = index;
			}
			else
			{
				Debug.LogError("Level index is out of range");
			}
		}

		public void DestroyPreviewLevel()
		{
			if(m_editorPreviewLevel != null)
			{
				GameObject.DestroyImmediate(m_editorPreviewLevel.gameObject);
				m_editorPreviewLevelIndex = -1;
			}
		}
#endif
	}
}