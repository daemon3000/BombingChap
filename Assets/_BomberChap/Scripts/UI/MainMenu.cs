using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class MainMenu : MonoBehaviour 
	{
		[SerializeField]
		private UnityEngine.UI.GraphicRaycaster m_graphicsRaycaster;

		[SerializeField]
		private RectTransform m_mainPage;

		[SerializeField]
		private RectTransform m_singlePlayerPage;

		private void Start()
		{
			ShowMainPage();
		}

		public void ShowMainPage()
		{
			m_mainPage.gameObject.SetActive(true);
			m_singlePlayerPage.gameObject.SetActive(false);
		}

		public void ShowSinglePlayerPage()
		{
			m_mainPage.gameObject.SetActive(false);
			m_singlePlayerPage.gameObject.SetActive(true);
		}

		public void DisableInput()
		{
			m_graphicsRaycaster.enabled = false;
		}

		public void EnableInput()
		{
			m_graphicsRaycaster.enabled = true;
		}
	}
}