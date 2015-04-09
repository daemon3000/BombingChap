using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BomberChapEditor
{
	public static class MenuShortcuts
	{
		[MenuItem("BomberChap/Delete Player Prefs")]
		private static void DeletePlayerPrefs()
		{
			PlayerPrefs.DeleteAll();
		}

		[MenuItem("BomberChap/Open Previous Scene")]
		private static void OpenPreviousScene()
		{
			string m_previousScene = EditorPrefs.GetString("MenuShortcuts.PreviousOpenScene", null);
			if(!string.IsNullOrEmpty(m_previousScene))
			{
				EditorApplication.isPlaying = false;
				EditorApplication.isPaused = false;
				EditorApplication.OpenScene(m_previousScene);
				m_previousScene = null;
			}
			else
			{
				Debug.Log("There is no previous scene to load");
			}
		}

		[MenuItem("BomberChap/Play Game")]
		private static void PlayGame()
		{
			EditorPrefs.SetString("MenuShortcuts.PreviousOpenScene", EditorApplication.currentScene);
			EditorApplication.isPlaying = false;
			EditorApplication.isPaused = false;
			EditorApplication.OpenScene("Assets/_BomberChap/Scenes/main.unity");
			EditorApplication.isPlaying = true;
		}
	}
}