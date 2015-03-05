using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(Button))]
	public class QuitGameOnClick : MonoBehaviour 
	{
		private Button m_button;
		
		private void Awake()
		{
			m_button = GetComponent<Button>();
			m_button.onClick.AddListener(HandleOnClick);
		}
		
		private void OnDestroy()
		{
			if(m_button != null)
				m_button.onClick.RemoveListener(HandleOnClick);
		}
		
		private void HandleOnClick()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}