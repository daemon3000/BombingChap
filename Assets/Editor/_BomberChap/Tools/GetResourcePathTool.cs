using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BomberChapEditor
{
	public class GetResourcePathTool : EditorWindow
	{
		[SerializeField] 
		private UnityEngine.Object m_target;
		[SerializeField] 
		private string m_path = "";

		private void OnGUI()
		{
			var oldTarget = m_target;
			m_target = EditorGUILayout.ObjectField("Target", m_target, typeof(UnityEngine.Object), false);

			if(m_target != oldTarget)
				ExtractTargetPath();

			EditorGUILayout.TextField("Path", m_path);
		}

		private void ExtractTargetPath()
		{
			string path = "";
			m_path = "";

			if(m_target == null)
				return;

			path = AssetDatabase.GetAssetPath(m_target);
			int i = path.IndexOf("Resources");
			if(i < 0)
			{
				Debug.LogWarning("The asset you selected is not in a Resources folder");
				return;
			}

			if(i + 10 >= path.Length)
				return;

			path = path.Substring(i + 10);
			i = path.LastIndexOf('.');
			if(i > 0)
			{
				path = path.Substring(0, i);
			}

			m_path = path;
		}

		[MenuItem("BomberChap/Tools/Get Resource Path")]
		private static void Open()
		{
			EditorWindow.GetWindow<GetResourcePathTool>();
		}
	}
}