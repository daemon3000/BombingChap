using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(Button))]
	[RequireComponent(typeof(Text))]
	public class ToggleMusicOnClick : MonoBehaviour 
	{
		[SerializeField]
		private AudioMixer m_audioMixer;

		private Button m_button;
		private Text m_text;

		private void Awake()
		{
			m_button = GetComponent<Button>();
			m_button.onClick.AddListener(HandleOnClick);
			m_text = GetComponent<Text>();

			int isMusicOn = PlayerPrefs.GetInt(PlayerPrefsKeys.MUSIC, 1);
			m_text.text = isMusicOn > 0 ? "MUSIC: ON" : "MUSIC: OFF";
			SetMusic(isMusicOn > 0);
		}

		private void OnDestroy()
		{
			if(m_button != null)
				m_button.onClick.RemoveListener(HandleOnClick);
		}

		private void HandleOnClick()
		{
			int isMusicOn = PlayerPrefs.GetInt(PlayerPrefsKeys.MUSIC, 1);
			if(isMusicOn > 0)
			{
				isMusicOn = 0;
				PlayerPrefs.SetInt(PlayerPrefsKeys.MUSIC, 0);
			}
			else
			{
				isMusicOn = 1;
				PlayerPrefs.SetInt(PlayerPrefsKeys.MUSIC, 1);
			}
			m_text.text = isMusicOn > 0 ? "MUSIC: ON" : "MUSIC: OFF";
			SetMusic(isMusicOn > 0);
		}

		private void SetMusic(bool enabled)
		{
			if(enabled)
			{
				AudioMixerSnapshot s = m_audioMixer.FindSnapshot("MusicOn");
				s.TransitionTo(1f);
			}
			else
			{
				AudioMixerSnapshot s = m_audioMixer.FindSnapshot("MusicOff");
				s.TransitionTo(1f);
			}
		}
	}
}