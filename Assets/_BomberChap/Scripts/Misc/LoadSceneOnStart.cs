using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class LoadSceneOnStart : MonoBehaviour 
	{
		[SerializeField]
		private string m_sceneName;

		private void Start()
		{
			Application.LoadLevel(m_sceneName);
		}
	}
}