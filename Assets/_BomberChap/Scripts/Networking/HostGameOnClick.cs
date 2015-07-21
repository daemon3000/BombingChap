using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace BomberChap
{
	public class HostGameOnClick : Photon.PunBehaviour 
	{
		[SerializeField]
		private InputField m_playerName;
		[SerializeField]
		private bool m_isRoomVisible;
		[SerializeField]
		private bool m_isRoomOpen;
		[SerializeField]
		private byte m_maxPlayersInRoom;

		public UnityEngine.Events.UnityEvent onHostGame;
		public UnityEngine.Events.UnityEvent onHostGameSuccess;
		public UnityEngine.Events.UnityEvent onHostGameFail;
		
		private bool m_isTryingToHostGame = false;
		
		public void HostGame()
		{
			if(!m_isTryingToHostGame)
			{
				RoomOptions roomOptions = new RoomOptions();
				roomOptions.isVisible = m_isRoomVisible;
				roomOptions.isOpen = m_isRoomOpen;
				roomOptions.maxPlayers = m_maxPlayersInRoom;
				
				m_isTryingToHostGame = true;
				onHostGame.Invoke();
				PhotonNetwork.CreateRoom(m_playerName.text, roomOptions, TypedLobby.Default);
			}
		}
		
		public override void OnJoinedRoom()
		{
			m_isTryingToHostGame = false;
			onHostGameSuccess.Invoke();
		}
		
		public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
		{
			m_isTryingToHostGame = false;
			onHostGameFail.Invoke();
		}
	}
}