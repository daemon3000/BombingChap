using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BomberChap
{
	public class DisconnectFromServerOnStart : MonoBehaviour 
	{
		private void Start()
		{
			PhotonNetwork.Disconnect();
		}
	}
}