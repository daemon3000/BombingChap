using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	public class SetScoreText : MonoBehaviour 
	{
		[SerializeField]
		private Text m_scoreText;

		[SerializeField]
		private Color m_normalColor = Color.white;

		[SerializeField]
		private Color m_bestScoreColor = Color.white;

		private void Start()
		{
			int bestScore = PlayerPrefs.GetInt(PlayerPrefsKeys.BEST_SCORE, 0);
			if(SinglePlayerGameController.Score > bestScore)
			{
				m_scoreText.text = "NEW BEST SCORE: " + SinglePlayerGameController.Score;
				m_scoreText.color = m_bestScoreColor;
			}
			else
			{
				m_scoreText.text = "SCORE: " + SinglePlayerGameController.Score;
				m_scoreText.color = m_normalColor;
			}
		}
	}
}