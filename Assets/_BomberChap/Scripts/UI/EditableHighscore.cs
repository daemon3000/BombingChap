using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	public class EditableHighscore : MonoBehaviour 
	{
		[SerializeField]
		private Text m_number;
		
		[SerializeField]
		private Text m_score;
		
		[SerializeField]
		private InputField m_playerName;

		public string PlayerName
		{
			get { return m_playerName.text; }
		}

		public int Score { get; private set; }

		public void Initialize(int number, int score)
		{
			m_number.text = number.ToString() + ".";
			m_score.text = score.ToString();
			Score = score;
		}
	}
}