using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(Button))]
	public class QuitGameOnClick : MonoBehaviour 
	{
		[SerializeField]
		private float m_delay;

		[SerializeField]
		private UnityEvent m_onStartQuit;

		private Button m_button;
		
		private void Awake()
		{
			m_button = GetComponent<Button>();
			m_button.onClick.AddListener(HandleOnClick);
			
#if UNITY_WEBPLAYER && !UNITY_EDITOR
			gameObject.SetActive(false);
#endif
		}
		
		private void OnDestroy()
		{
			if(m_button != null)
				m_button.onClick.RemoveListener(HandleOnClick);
		}
		
		private void HandleOnClick()
		{
			StartCoroutine(QuitGame());
		}

		private IEnumerator QuitGame()
		{
			m_onStartQuit.Invoke();
			yield return new WaitForSeconds(m_delay);

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}