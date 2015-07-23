using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace BomberChap
{
	public class OnlineMultiplayerGameController : MultiplayerGameControllerBase
	{
		[SerializeField]
		private ScreenFader m_screenFader;
		[SerializeField]
		private Canvas m_canvas;
		[SerializeField]
		private Text m_roundText;
		[SerializeField]
		private float m_roundTexDuration;
		[SerializeField]
		private int m_maxRounds;
		[SerializeField]
		private float m_nextRoundDelay;

		public UnityEngine.Events.UnityEvent onStartNextRound;
		public UnityEngine.Events.UnityEvent onPlayerDisconnected;
		public UnityEngine.Events.UnityEvent onDisconnectedFromServer;

		private PhotonView m_photonView;
		private int m_levelIndex;
		private int m_currentRound;
		private int m_playerOneWins;
		private int m_playerTwoWins;
		private bool m_registeredDeathThisRound;

		public override int MaxRounds 
		{
			get { return m_maxRounds; }
		}

		public override int PlayerOneWins
		{
			get { return m_playerOneWins; }
		}

		public override int PlayerTwoWins
		{
			get { return m_playerTwoWins; }
		}

		private void Start()
		{
			m_photonView = PhotonView.Get(gameObject);
			m_levelIndex = 0;
			m_currentRound = 0;
			m_playerOneWins = 0;
			m_playerTwoWins = 0;

			if(PhotonNetwork.isMasterClient)
			{
				m_levelIndex = UnityEngine.Random.Range(0, LevelManager.LevelCount);
				m_photonView.RPC("OnSetLevelIndexRPC", PhotonTargets.AllBuffered, m_levelIndex);
			}

			StartCoroutine(StartNextRound(0.0f));
		}

		private IEnumerator StartNextRound(float delay)
		{
			ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
			properties.Add("IsDoneLoadingLevel", false);
			PhotonNetwork.player.SetCustomProperties(properties);

			if(delay > 0.0f)
				yield return new WaitForSeconds(delay);
			else
				yield return null;
			
			if(m_currentRound == m_maxRounds || m_playerOneWins > m_maxRounds / 2 || m_playerTwoWins > m_maxRounds / 2)
			{
				Globals.SetBool(GlobalKeys.PLAYER_ONE_WON, m_playerOneWins > m_playerTwoWins);
				Globals.SetBool(GlobalKeys.PLAYER_TWO_WON, m_playerTwoWins > m_playerOneWins);
				NotificationCenter.Dispatch(Notifications.ON_MULTI_PLAYER_MATCH_OVER);
				yield break;
			}

			onStartNextRound.Invoke();

			if(m_screenFader != null)
				yield return m_screenFader.FadeOut();
			else
				yield return null;
			
			m_currentRound++;
			m_roundText.text = "ROUND " + m_currentRound;
			m_canvas.gameObject.SetActive(true);
			m_screenFader.ClearFade();
			LevelManager.LoadLevel(m_levelIndex, false);

			properties["IsDoneLoadingLevel"] = true;
			PhotonNetwork.player.SetCustomProperties(properties);

			float elapsedTime = 0.0f;
			while(elapsedTime < m_roundTexDuration)
			{
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}

			PhotonPlayer[] otherPlayers = PhotonNetwork.otherPlayers;
			object otherPlayersDoneLoading = false;
			while(!(bool)otherPlayersDoneLoading)
			{
				foreach(PhotonPlayer op in otherPlayers)
				{
					op.customProperties.TryGetValue("IsDoneLoadingLevel", out otherPlayersDoneLoading);
					if(otherPlayersDoneLoading == null || !(bool)otherPlayersDoneLoading)
						break;
				}
				yield return null;
			}

			NotificationCenter.Dispatch(Notifications.ON_ALL_PLAYERS_LOADED_ONLINE_GAME_LEVEL);

			m_canvas.gameObject.SetActive(false);
			
			if(m_screenFader != null)
				yield return m_screenFader.FadeIn();
			else
				yield return null;

			m_screenFader.ClearFade();
			m_registeredDeathThisRound = false;
		}

		private void OnPlayerDead()
		{
			if(PhotonNetwork.isMasterClient)
				m_photonView.RPC("OnPlayerOneDeadRPC", PhotonTargets.All);
			else
				m_photonView.RPC("OnPlayerTwoDeadRPC", PhotonTargets.All);
		}

		public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			onPlayerDisconnected.Invoke();
		}

		public void OnDisconnectedFromPhoton()
		{
			onDisconnectedFromServer.Invoke();
		}

		[PunRPC]
		private void OnSetLevelIndexRPC(int levelIndex)
		{
			m_levelIndex = levelIndex;
		}

		[PunRPC]
		private void OnPlayerOneDeadRPC()
		{
			m_playerTwoWins++;
			if(!m_registeredDeathThisRound)
			{
				m_registeredDeathThisRound = true;
				StartCoroutine(StartNextRound(m_nextRoundDelay));
			}
		}

		[PunRPC]
		private void OnPlayerTwoDeadRPC()
		{
			m_playerOneWins++;
			if(!m_registeredDeathThisRound)
			{
				m_registeredDeathThisRound = true;
				StartCoroutine(StartNextRound(m_nextRoundDelay));
			}
		}
	}
}
