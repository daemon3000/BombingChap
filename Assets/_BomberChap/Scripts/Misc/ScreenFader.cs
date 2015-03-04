using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class ScreenFader : MonoBehaviour 
	{
		[SerializeField]
		private GameObject m_guiTexturePrefab;
		[SerializeField]
		private float m_fadeTime;
		[SerializeField]
		private bool m_ignoreTimescale;

		private GUITexture m_guiTexture;
		private Color m_colorFadeOut;
		private Color m_colorFadeIn;

		private void Awake()
		{
			GameObject go = GameObject.Instantiate(m_guiTexturePrefab) as GameObject;
			m_guiTexture = go.GetComponent<GUITexture>();
			m_guiTexture.gameObject.SetActive(false);
			m_colorFadeOut = new Color(0.0f, 0.0f, 0.0f, 1.0f);
			m_colorFadeIn = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		}

		public Coroutine FadeOut()
		{
			return StartCoroutine(FadeOutInternal());
		}

		private IEnumerator FadeOutInternal()
		{
			m_guiTexture.gameObject.SetActive(true);
			m_guiTexture.color = m_colorFadeIn;
			m_guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);

			float l = 0.0f;
			float speed = 1.0f / m_fadeTime;

			while(l < 1.0f)
			{
				l += speed * (m_ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime);
				m_guiTexture.color = Color.Lerp(m_colorFadeIn, m_colorFadeOut, l);
				yield return null;
			}
		}

		public Coroutine FadeIn()
		{
			return StartCoroutine(FadeInInternal());
		}

		private IEnumerator FadeInInternal()
		{
			m_guiTexture.gameObject.SetActive(true);
			m_guiTexture.color = m_colorFadeOut;
			m_guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
			
			float l = 0.0f;
			float speed = 1.0f / m_fadeTime;
			
			while(l < 1.0f)
			{
				l += speed * (m_ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime);
				m_guiTexture.color = Color.Lerp(m_colorFadeOut, m_colorFadeIn, l);
				yield return null;
			}
		}

		public void ClearFade()
		{
			StopAllCoroutines();
			m_guiTexture.gameObject.SetActive(false);
		}
	}
}