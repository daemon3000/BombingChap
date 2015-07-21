using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	public class DisconnectFromServerOnClick : MonoBehaviour 
	{
		[SerializeField]
		private bool m_subscribeAutomatically = true;
		private Button m_button;
		
		private void Awake()
		{
			m_button = GetComponent<Button>();
			if(m_button != null && m_subscribeAutomatically)
				m_button.onClick.AddListener(DisconnectFromServer);
		}
		
		private void OnDestroy()
		{
			if(m_button != null && m_subscribeAutomatically)
				m_button.onClick.RemoveListener(DisconnectFromServer);
		}
		
		public void DisconnectFromServer()
		{
			PhotonNetwork.Disconnect();
		}
	}
}