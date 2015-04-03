using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BomberChap
{
	public class AudioManager : MonoBehaviour 
	{
		[SerializeField]
		private int m_maxAudioPoolSize;

		[SerializeField]
		private GameObject m_template;

		private Queue<AudioSource> m_audioPool;
		private List<AudioSource> m_activeAudioSources;
		private int m_audioSourceCount;
		private static AudioManager m_instance;

		private void Awake()
		{
			if(m_instance != null)
				Debug.LogWarning("You shouldn't have more than one AudioManager per scene");

			m_instance = this;
			m_audioPool = new Queue<AudioSource>(m_maxAudioPoolSize + 1);
			m_activeAudioSources = new List<AudioSource>(m_maxAudioPoolSize + 1);
			m_audioSourceCount = 0;
		}

		private void Update()
		{
			for(int i = 0; i < m_activeAudioSources.Count; i++)
			{
				if(!m_activeAudioSources[i].isPlaying)
				{
					m_audioPool.Enqueue(m_activeAudioSources[i]);
					m_activeAudioSources.RemoveAt(i);
					i--;
				}
			}
		}

		private AudioSource InternalPlaySound(AudioClip sound)
		{
			AudioSource audioSource = GetAudioSource();
			if(audioSource != null)
			{
				audioSource.clip = sound;
				audioSource.Play();
				m_activeAudioSources.Add(audioSource);
			}

			return audioSource;
		}

		private AudioSource GetAudioSource()
		{
			return m_audioPool.Count > 0 ? m_audioPool.Dequeue() : CreateNewAudioSource();
		}
		
		private AudioSource CreateNewAudioSource()
		{
			if(m_template == null || m_audioSourceCount >= m_maxAudioPoolSize)
				return null;
			
			GameObject gameObj = GameObject.Instantiate(m_template) as GameObject;
			gameObj.transform.SetParent(transform, true);
			m_audioSourceCount++;
			return gameObj.GetComponent<AudioSource>();
		}

		public static bool Exists
		{
			get { return m_instance != null; }
		}

		public static AudioSource PlaySound(AudioClip sound)
		{
			if(m_instance == null)
				return null;

			if(sound == null) 
			{
				Debug.LogError("Can't play a null sound");
				return null;
			}

			return m_instance.InternalPlaySound(sound);
		}
	}
}