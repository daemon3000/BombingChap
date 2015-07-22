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
		[SerializeField]
		private string m_gameSceneName;

		public UnityEngine.Events.UnityEvent onDisconnectedFromServer;

		private void Start()
		{
			m_roomName.text = PhotonNetwork.room.name.ToUpperInvariant();
			m_roomStatus.text = PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers ? m_roomFullStatus : m_roomNotFullStatus;
			m_startGameButton.gameObject.SetActive(PhotonNetwork.isMasterClient);
			m_startGameButton.interactable = PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers;

			if(PhotonNetwork.isMasterClient && !PhotonNetwork.room.open)
				PhotonNetwork.room.open = true;
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

			if(PhotonNetwork.isMasterClient && !PhotonNetwork.room.open)
				PhotonNetwork.room.open = true;
		}

		public override void OnDisconnectedFromPhoton()
		{
			onDisconnectedFromServer.Invoke();
		}

		public void StartGame()
		{
			if(PhotonNetwork.isMasterClient)
			{
				PhotonNetwork.room.open = false;
				photonView.RPC("OnStartGameRPC", PhotonTargets.All);
			}
			else
			{
				Debug.LogError("Only the host can start an online match");
			}
		}

		[PunRPC]
		private void OnStartGameRPC()
		{
			Application.LoadLevel(m_gameSceneName);
		}
	}
}