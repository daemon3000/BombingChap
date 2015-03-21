using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BomberChap
{
	public class NotificationCenter : MonoBehaviour 
	{
		private Dictionary<string, HashSet<GameObject>> m_observers;
		private static NotificationCenter m_instance;
		
		private void Awake()
		{
			m_observers = new Dictionary<string, HashSet<GameObject>>();
			m_instance = this;
		}
		
		private void OnDestroy()
		{
			m_observers = null;
			m_instance = null;
		}

		public static bool Exists
		{
			get { return m_instance != null; }
		}
		
		public static void AddObserver(GameObject observer, string message)
		{
			if(m_instance == null)
				throw new System.NullReferenceException("Notification center instance is null");
			
			HashSet<GameObject> hs = null;
			if(m_instance.m_observers.TryGetValue(message, out hs))
			{
				if(!hs.Contains(observer))
					hs.Add(observer);
			}
			else
			{
				hs = new HashSet<GameObject>();
				hs.Add(observer);
				m_instance.m_observers.Add(message, hs);
			}
		}
		
		public static void AddObserver(GameObject observer, string[] messages)
		{
			if(m_instance == null)
				throw new System.NullReferenceException("Notification center instance is null");
			
			HashSet<GameObject> hs = null;
			foreach(string message in messages)
			{
				if(m_instance.m_observers.TryGetValue(message, out hs))
				{
					if(!hs.Contains(observer))
						hs.Add(observer);
				}
				else
				{
					hs = new HashSet<GameObject>();
					hs.Add(observer);
					m_instance.m_observers.Add(message, hs);
				}
			}
		}
		
		public static void RemoveObserver(GameObject observer, string message)
		{
			if(m_instance == null)
				throw new System.NullReferenceException("Notification center instance is null");
			
			HashSet<GameObject> hs = null;
			if(m_instance.m_observers.TryGetValue(message, out hs))
			{
				hs.Remove(observer);
			}
		}
		
		public static void RemoveObserver(GameObject observer, string[] messages)
		{
			if(m_instance == null)
				throw new System.NullReferenceException("Notification center instance is null");
			
			HashSet<GameObject> hs = null;
			foreach(string message in messages)
			{
				if(m_instance.m_observers.TryGetValue(message, out hs))
				{
					hs.Remove(observer);
				}
			}
		}
		
		public static void Dispatch(string message)
		{
			if(m_instance == null)
				throw new System.NullReferenceException("Notification center instance is null");
			
			HashSet<GameObject> hs = null;
			if(m_instance.m_observers.TryGetValue(message, out hs))
			{
				foreach(GameObject obs in hs)
				{
					obs.SendMessage(message, SendMessageOptions.DontRequireReceiver);
				}
			}
		}

		public static void Dispatch(string message, object arg, bool requireReceiver)
		{
			if(m_instance == null)
				throw new System.NullReferenceException("Notification center instance is null");

			SendMessageOptions options = requireReceiver ? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver;
			HashSet<GameObject> hs = null;
			if(m_instance.m_observers.TryGetValue(message, out hs))
			{
				foreach(GameObject obs in hs)
				{
					if(arg != null)
						obs.SendMessage(message, arg, options);
					else
						obs.SendMessage(message, options);
				}
			}
		}
	}
}