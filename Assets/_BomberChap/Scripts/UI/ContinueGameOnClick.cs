using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(Button))]
	public class ContinueGameOnClick : MonoBehaviour 
	{
		[SerializeField]
		private string m_singlePlayerSceneName;

		[SerializeField]
		private float m_delay;
		
		[SerializeField]
		private UnityEngine.Events.UnityEvent m_onContinue;
		
		private Button m_button;
		
		private void Awake()
		{
			m_button = GetComponent<Button>();
			m_button.onClick.AddListener(HandleOnClick);

			int gameLevel = PlayerPrefs.GetInt(PlayerPrefsKeys.GAME_LEVEL, -1);
			m_button.interactable = gameLevel >= 0;
		}
		
		private void OnDestroy()
		{
			if(m_button != null)
				m_button.onClick.RemoveListener(HandleOnClick);
		}
		
		private void HandleOnClick()
		{
			StartCoroutine(ContinueGame());
		}

		private IEnumerator ContinueGame()
		{
			m_onContinue.Invoke();
			yield return new WaitForSeconds(m_delay);

			Application.LoadLevel(m_singlePlayerSceneName);
		}
	}
}