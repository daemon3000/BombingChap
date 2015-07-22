using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class DestroyNetworkObjectOnLevelUnload : Photon.MonoBehaviour 
	{
		private void OnGameLevelWillUnload()
		{
			if(photonView.isMine)
				PhotonNetwork.Destroy(gameObject);
		}
	}
}