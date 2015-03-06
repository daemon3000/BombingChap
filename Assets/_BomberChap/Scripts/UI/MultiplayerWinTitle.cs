using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	[RequireComponent(typeof(Text))]
	public class MultiplayerWinTitle : MonoBehaviour 
	{
		private void Start()
		{
			Text text = GetComponent<Text>();
			if(MultiplayerGameController.PlayerOneWon)
				text.text = "PLAYER ONE WINS!";
			else if(MultiplayerGameController.PlayerTwoWon)
				text.text = "PLAYER TWO WINS!";
			else
				text.text = "GAME DRAW!";
		}
	}
}