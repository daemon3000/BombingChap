using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	public class Highscore : MonoBehaviour 
	{
		[SerializeField]
		private Text m_number;

		[SerializeField]
		private Text m_score;

		[SerializeField]
		private Text m_playerName;

		[SerializeField]
		private int m_maxPlayerNameLength;

		public void Initialize(int number, string playerName, int score)
		{
			m_number.text = number.ToString() + ".";
			m_playerName.text = playerName.Length > m_maxPlayerNameLength ? playerName.Substring(0, m_maxPlayerNameLength) : playerName;
			m_score.text = score.ToString();
		}
	}
}