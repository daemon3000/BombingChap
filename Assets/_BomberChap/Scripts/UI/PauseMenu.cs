using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class PauseMenu : MonoBehaviour 
	{
		[SerializeField]
		private Canvas m_canvas;

		private bool m_isOpen = false;

		private void Update()
		{
			if(Input.GetButtonDown("Pause"))
			{
				if(!m_isOpen)
					Open();
			}
		}

		public void Open()
		{
			if(!m_isOpen)
			{
				PauseManager.Pause();
				m_canvas.gameObject.SetActive(true);
				m_isOpen = true;
			}
		}

		public void Close()
		{
			if(m_isOpen)
			{
				PauseManager.UnPause();
				m_canvas.gameObject.SetActive(false);
				m_isOpen = false;
			}
		}
	}
}