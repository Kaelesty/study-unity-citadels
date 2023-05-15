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
    public string character;

    private bool nicknameSetted = false;
    private bool shareRecieved = false;

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
        nicknameSetted = true;
        shareRecieved = true;
        nickname = newNickname;
    }

    public bool checkView()
    {
        return view.IsMine;
    }

    private void Update()
    {
        if (checkView() && nicknameSetted)
        {
            view.RPC("originalSharing", RpcTarget.Others, nickname, id);
        }
    }

    [PunRPC]
    private void originalSharing(string orNickname, int orID)
    {
        if (!shareRecieved)
        {
            shareRecieved = true;
            nickname = orNickname;
            id = orID;
            var csm = GameObject.FindGameObjectWithTag("ConSM").GetComponent<ConnectingScreenManager>();
            csm.addNicknameLabel(orNickname);
        }
    }

    public void callSetCharacter(string preset)
    {
        view.RPC("setCharacter", RpcTarget.All, preset);
    }

    [PunRPC]
    private void setCharacter(string preset)
    {
        character = preset;
    }
}
