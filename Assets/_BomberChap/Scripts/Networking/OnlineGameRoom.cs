using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace BomberChap
{
	public class OnlineGameRoom : Photon.PunBehaviour 
	{
		[SerializeField]
		private Text m_roomName;
		[SerializeField]
		private Text m_roomStatus;
		[SerializeField]
		private Button m_startGameButton;
		[SerializeField]
		private string m_roomFullStatus;
		[SerializeField]
		private string m_roomNotFullStatus;

		public UnityEngine.Events.UnityEvent onDisconnectedFromServer;

		private void Start()
		{
			m_roomName.text = string.Format("{0}'S GAME", PhotonNetwork.room.name.ToUpperInvariant());
			m_roomStatus.text = PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers ? m_roomFullStatus : m_roomNotFullStatus;
			m_startGameButton.gameObject.SetActive(PhotonNetwork.isMasterClient);
			m_startGameButton.interactable = PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers;
		}

		public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
		{
			m_roomStatus.text = PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers ? m_roomFullStatus : m_roomNotFullStatus;
			m_startGameButton.gameObject.SetActive(PhotonNetwork.isMasterClient);
			m_startGameButton.interactable = PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers;
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			m_roomStatus.text = PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers ? m_roomFullStatus : m_roomNotFullStatus;
			m_startGameButton.gameObject.SetActive(PhotonNetwork.isMasterClient);
			m_startGameButton.interactable = PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers;
		}

		public override void OnDisconnectedFromPhoton()
		{
			onDisconnectedFromServer.Invoke();
		}
	}
}