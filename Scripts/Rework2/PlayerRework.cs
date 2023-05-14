using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRework : MonoBehaviourPunCallbacks
{
    PhotonView view;
    public int id;
    public string nickname;

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

    public void callSetNickname(string nickname)
    {
        view.RPC("setNickname", RpcTarget.All, nickname);
    }

    [PunRPC]
    private void setNickname(string newNickname)
    {
        nickname = newNickname;
    }

    public bool checkView()
    {
        return view.IsMine;
    }
}
