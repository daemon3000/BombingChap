using UnityEngine;
using System.Collections.Generic;

namespace BomberChap
{
	public class HighscoreScreen : MonoBehaviour 
	{
		[SerializeField]
		private RectTransform m_highscoreLayout;

		[SerializeField]
		private GameObject m_normalHighscorePrefab;

		[SerializeField]
		private GameObject m_editableHighscorePrefab;

		[SerializeField]
		private int m_numberOfHighscores;

		private EditableHighscore m_editableHighscore;
		private int m_editableHighscoreIndex;

		private void Start()
		{
			List<string> highscoreNames = new List<string>();
			List<int> highscoreValues = new List<int>();

			m_editableHighscore = null;
			m_editableHighscoreIndex = -1;

			for(int i = 0; i < m_numberOfHighscores; i++)
			{
				string name = PlayerPrefs.GetString(string.Format(PlayerPrefsKeys.HIGHSCORE_NAME, i), GlobalConstants.DEFAULT_HIGHSCORE_NAME);
				int score = PlayerPrefs.GetInt(string.Format(PlayerPrefsKeys.HIGHSCORE_VALUE, i), 0);

				if(string.IsNullOrEmpty(name))
					name = GlobalConstants.DEFAULT_HIGHSCORE_NAME;

				highscoreNames.Add(name);
				highscoreValues.Add(score);
			}

			for(int i = 0; i < highscoreValues.Count; i++)
			{
				if(SinglePlayerGameController.Score > highscoreValues[i])
				{
					for(int k = highscoreValues.Count - 1; k > i; k--)
					{
						highscoreValues[k] = highscoreValues[k - 1];
						highscoreNames[k] = highscoreNames[k - 1];
					}
					highscoreValues[i] = SinglePlayerGameController.Score;
					highscoreNames[i] = GlobalConstants.DEFAULT_HIGHSCORE_NAME;
					m_editableHighscoreIndex = i;
					break;
				}
			}

			for(int i = 0; i < highscoreValues.Count; i++)
			{
				if(i == m_editableHighscoreIndex)
				{
					GameObject go = GameObject.Instantiate<GameObject>(m_editableHighscorePrefab);
					go.transform.SetParent(m_highscoreLayout);
					go.transform.SetAsLastSibling();
					go.transform.localScale = Vector3.one;
					m_editableHighscore = go.GetComponent<EditableHighscore>();
					m_editableHighscore.Initialize(i + 1, highscoreValues[i]);
				}
				else
				{
					GameObject go = GameObject.Instantiate<GameObject>(m_normalHighscorePrefab);
					go.transform.SetParent(m_highscoreLayout);
					go.transform.SetAsLastSibling();
					go.transform.localScale = Vector3.one;
					Highscore hs = go.GetComponent<Highscore>();
					hs.Initialize(i + 1, highscoreNames[i], highscoreValues[i]);
				}
			}
		}

		public void SaveHighscores()
		{
			if(m_editableHighscoreIndex >= 0)
			{
				string name = m_editableHighscore.PlayerName;
				if(string.IsNullOrEmpty(name))
					name = GlobalConstants.DEFAULT_HIGHSCORE_NAME;

				PlayerPrefs.SetString(string.Format(PlayerPrefsKeys.HIGHSCORE_NAME, m_editableHighscoreIndex), name);
				PlayerPrefs.SetInt(string.Format(PlayerPrefsKeys.HIGHSCORE_VALUE, m_editableHighscoreIndex), m_editableHighscore.Score);
			}
		}

		public void ResetGameProgression()
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.SP_GAME_LEVEL, -1);
			PlayerPrefs.SetInt(PlayerPrefsKeys.SP_SCORE, 0);
			PlayerPrefs.SetInt(PlayerPrefsKeys.SP_BOMB_COUNT, GlobalConstants.MIN_BOMB_COUNT);
			PlayerPrefs.SetInt(PlayerPrefsKeys.SP_BOMB_RANGE, GlobalConstants.MIN_BOMB_RANGE);
			PlayerPrefs.SetFloat(PlayerPrefsKeys.SP_PLAYER_SPEED, GlobalConstants.MIN_PLAYER_SPEED);
		}
	}
}