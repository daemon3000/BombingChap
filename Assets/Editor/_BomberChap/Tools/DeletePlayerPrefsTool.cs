using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BomberChapEditor
{
	public class DeletePlayerPrefsTool
	{
		[MenuItem("BomberChap/Delete Player Prefs")]
		private static void DeletePlayerPrefs()
		{
			PlayerPrefs.DeleteAll();
		}
	}
}