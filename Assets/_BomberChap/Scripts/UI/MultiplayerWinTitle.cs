using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(Text))]
	public class MultiplayerWinTitle : MonoBehaviour 
	{
		[SerializeField]
		private bool m_isOnlineMatch;

		private void Start()
		{
			Text text = GetComponent<Text>();
			bool playerOneWon = Globals.GetBool(GlobalKeys.PLAYER_ONE_WON, false);
			bool playerTwoWon = Globals.GetBool(GlobalKeys.PLAYER_TWO_WON, false);
			if(playerOneWon)
				text.text = "PLAYER ONE WINS!";
			else if(playerTwoWon)
				text.text = "PLAYER TWO WINS!";
			else
				text.text = "GAME DRAW!";
		}
	}
}