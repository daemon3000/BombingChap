using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class NotificationReceiver : MonoBehaviour 
	{
		[SerializeField] private string[] m_messages;
		
		private void Awake()
		{
			NotificationCenter.AddObserver(gameObject, m_messages);
		}
		
		private void OnDestroy()
		{
			if(NotificationCenter.Exists)
				NotificationCenter.RemoveObserver(gameObject, m_messages);
		}
	}
}