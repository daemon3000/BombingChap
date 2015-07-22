using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class MP_PauseMenu : MonoBehaviour 
	{
		[SerializeField]
		private Canvas m_canvas;
		[SerializeField]
		private UnityEngine.UI.GraphicRaycaster m_graphicsRaycaster;
		
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
				m_canvas.gameObject.SetActive(true);
				m_isOpen = true;
				EnableInput();
			}
		}
		
		public void Close()
		{
			if(m_isOpen)
			{
				m_canvas.gameObject.SetActive(false);
				m_isOpen = false;
			}
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