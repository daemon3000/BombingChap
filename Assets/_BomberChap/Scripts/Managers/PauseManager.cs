using UnityEngine;
using System;
using System.Collections.Generic;

namespace BomberChap
{
	public enum PauseManagerState
	{
		Pausing,
		UnPausing,
		Idle,
		Paused
	}

	public sealed class PauseManager : MonoBehaviour 
	{
		[SerializeField] private bool m_dontDestroyOnLoad;
		private PauseManagerState m_state;
		private float m_lastTimeScale;
		private bool m_hardPause;
		private static PauseManager m_instance;
		
		public static PauseManager Instance
		{
			get
			{
				return m_instance;
			}
		}
		
		public static PauseManagerState State
		{
			get
			{
				return m_instance.m_state;
			}
		}
		
		public static bool IsPaused
		{
			get
			{
				return (m_instance.m_state == PauseManagerState.Paused);
			}
		}
		
		public static void Pause()
		{
			//	The game will be paused at the start of the next update cycle.
			if(m_instance.m_state == PauseManagerState.Idle)
			{
				m_instance.m_state = PauseManagerState.Pausing;
			}
		}
		
		public static void UnPause()
		{
			//	The game will be unpaused at the start of the next update cycle.
			if(m_instance.m_state == PauseManagerState.Paused)
			{
				m_instance.m_state = PauseManagerState.UnPausing;
			}
		}
		
		private void Awake()
		{
			if(m_instance != null)
			{
				Destroy(this);
			}
			else
			{
				m_instance = this;
				m_hardPause = false;
				m_lastTimeScale = Time.timeScale;
				m_state = PauseManagerState.Idle;
				if(m_dontDestroyOnLoad)
					DontDestroyOnLoad(gameObject);
			}
		}
		
		private void Update()
		{
			switch(m_state)
			{
			case PauseManagerState.Pausing:
				m_lastTimeScale = Time.timeScale;
				m_state = PauseManagerState.Paused;
				Time.timeScale = 0.0f;
				break;
			case PauseManagerState.UnPausing:
				m_state = PauseManagerState.Idle;
				Time.timeScale = m_lastTimeScale;
				break;
			default:
				break;
			}
			
			if(Input.GetButtonDown("Pause"))
			{
				if(m_state == PauseManagerState.Idle)
				{
					m_state = PauseManagerState.Pausing;
					m_hardPause = true;
				}
				else if(m_state == PauseManagerState.Paused)
				{
					if(m_hardPause)
					{
						m_state = PauseManagerState.UnPausing;
						m_hardPause = false;
					}
				}
			}
		}
		
		private void OnLevelWasLoaded(int levelIndex)
		{
			if(m_state != PauseManagerState.Idle)
			{
				Time.timeScale = m_lastTimeScale;
				m_state = PauseManagerState.Idle;
			}
		}
	}
}