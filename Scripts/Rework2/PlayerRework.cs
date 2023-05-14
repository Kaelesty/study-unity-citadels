using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRework : MonoBehaviourPunCallbacks
{
    PhotonView view;
    public int id;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        gameObject.tag = "Player";
        if (view.IsMine)
        {
            view.RPC("setID", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    [PunRPC]

    private void setID(int newID)
    {
        id = newID;
    }
}
