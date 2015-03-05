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
			Application.LoadLevel(m_singlePlayerSceneName);
		}
	}
}