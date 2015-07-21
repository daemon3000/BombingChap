using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class ConnectToServerOnStart : Photon.PunBehaviour 
	{
		public UnityEngine.Events.UnityEvent onConnectedToServer;
		public UnityEngine.Events.UnityEvent onDisconnectedFromServer;
		public UnityEngine.Events.UnityEvent onFailedToConnectToServer;

		private void Start()
		{
			PhotonNetwork.ConnectUsingSettings(GameVersion.VERSION);
		}

		public override void OnFailedToConnectToPhoton(DisconnectCause cause)
		{
			onFailedToConnectToServer.Invoke();
		}

		public override void OnConnectedToPhoton()
		{
			onConnectedToServer.Invoke();
		}

		public override void OnDisconnectedFromPhoton()
		{
			onDisconnectedFromServer.Invoke();
		}
	}
}