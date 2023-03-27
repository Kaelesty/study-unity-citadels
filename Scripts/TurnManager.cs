using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnManager : MonoBehaviourPunCallbacks
{
    PhotonView view;
    private int activePlayerID = 1;
    public int networkID = PhotonNetwork.LocalPlayer.ActorNumber;

    private void Awake()
    {
        view = GetComponent<PhotonView>();

    }    
    public void switchActivePlayerID()
    {
        activePlayerID += 1;
        if (PhotonNetwork.CurrentRoom.PlayerCount < activePlayerID)
        {
            activePlayerID = 1;
        }
        view.RPC("switchActivePlayerID_RPC", RpcTarget.Others);
    }

    [PunRPC]
    public void switchActivePlayerID_RPC()
    {
        activePlayerID += 1;
        if (PhotonNetwork.CurrentRoom.PlayerCount < activePlayerID)
        {
            activePlayerID = 1;
        }
    }

    public int getActivePlayerID()
    {
        return activePlayerID;
    }
}
