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

		private ExitGames.Client.Photon.Hashtable m_properties;

		private void Start()
		{
			m_roomName.text = PhotonNetwork.room.name.ToUpperInvariant();
			m_roomStatus.text = PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers ? m_roomFullStatus : m_roomNotFullStatus;
			m_startGameButton.gameObject.SetActive(PhotonNetwork.isMasterClient);
			m_startGameButton.interactable = CanStartGame();

			m_properties = new ExitGames.Client.Photon.Hashtable();
			m_properties.Add("IsInRoomLobby", true);
			PhotonNetwork.player.SetCustomProperties(m_properties);

			if(PhotonNetwork.isMasterClient && !PhotonNetwork.room.open)
				PhotonNetwork.room.open = true;
		}

		private void OnDestroy()
		{
			m_properties["IsInRoomLobby"] = false;
			PhotonNetwork.player.SetCustomProperties(m_properties);
		}

		private void Update()
		{
			if(PhotonNetwork.isMasterClient)
				m_startGameButton.interactable = CanStartGame();
		}

		private bool CanStartGame()
		{
			if(!PhotonNetwork.isMasterClient)
				return false;

			bool isRoomFull = PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers;
			bool areAllPlayersInLobby = true;
			object isPlayerInLobby = null;

			foreach(var p in PhotonNetwork.otherPlayers)
			{
				p.customProperties.TryGetValue("IsInRoomLobby", out isPlayerInLobby);
				if(isPlayerInLobby == null || !(bool)isPlayerInLobby)
				{
					areAllPlayersInLobby = false;
					break;
				}
			}

			return isRoomFull && areAllPlayersInLobby;
		}

		public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
		{
			m_roomStatus.text = PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers ? m_roomFullStatus : m_roomNotFullStatus;
			m_startGameButton.gameObject.SetActive(PhotonNetwork.isMasterClient);
			m_startGameButton.interactable = CanStartGame();
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			m_roomStatus.text = PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers ? m_roomFullStatus : m_roomNotFullStatus;
			m_startGameButton.gameObject.SetActive(PhotonNetwork.isMasterClient);
			m_startGameButton.interactable = CanStartGame();

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