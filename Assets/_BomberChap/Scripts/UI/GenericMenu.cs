using UnityEngine;
using System.Collections.Generic;

namespace BomberChap
{
	public class GenericMenu : MonoBehaviour 
	{
		[SerializeField]
		private UnityEngine.UI.GraphicRaycaster m_graphicsRaycaster;
		[SerializeField]
		private string m_startPage;
		[SerializeField]
		private List<RectTransform> m_menuPages;

		private RectTransform m_currentPage;

		private void Start()
		{
			m_currentPage = null;
			foreach(var p in m_menuPages)
				p.gameObject.SetActive(false);

			ShowPage(m_startPage);
		}

		public void ShowPage(string name)
		{
			if(m_currentPage != null)
				m_currentPage.gameObject.SetActive(false);

			m_currentPage = m_menuPages.Find(page => page.name == name);
			if(m_currentPage != null)
				m_currentPage.gameObject.SetActive(true);
			else
				Debug.LogWarningFormat("Could not find menu page: {0}", name);
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