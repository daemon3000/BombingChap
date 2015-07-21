using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	public class LoadSceneOnClick : MonoBehaviour 
	{
		[SerializeField]
		private string m_sceneName;
		[SerializeField]
		private float m_delay;
		[SerializeField]
		private bool m_ignoreTimescale;
		[SerializeField]
		private bool m_subscribeAutomatically = true;

		private Button m_button;

		private void Awake()
		{
			m_button = GetComponent<Button>();
			if(m_button != null && m_subscribeAutomatically)
				m_button.onClick.AddListener(LoadScene);
		}

		private void OnDestroy()
		{
			if(m_button != null && m_subscribeAutomatically)
				m_button.onClick.RemoveListener(LoadScene);
		}

		public void LoadScene()
		{
			StartCoroutine(LoadSceneInternal());
		}

		private IEnumerator LoadSceneInternal()
		{
			if(m_ignoreTimescale)
			{
				float f = 0.0f;
				while(f < m_delay)
				{
					f += Time.unscaledDeltaTime;
					yield return null;
				}
			}
			else
			{
				yield return new WaitForSeconds(m_delay);
			}
			Application.LoadLevel(m_sceneName);
		}
	}
}