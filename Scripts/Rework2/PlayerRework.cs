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

    public bool characterShown;

    private int balance = 0;

    public List<string> districts = new List<string>();
    public List<string> buildedDistricts = new List<string>();

    private bool nicknameSetted = false;
    private bool shareRecieved = false;

    private bool assassinMarker = false;

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

    public void callAddDistrict(string preset)
    {
        view.RPC("addDistrict", RpcTarget.All, preset);
    }

    public void callIncreaseBalance(int amount)
    {
        view.RPC("increaseBalance", RpcTarget.All, amount);
    }

    public void callBuildDistrict(string preset)
    {
        view.RPC("buildDistrict", RpcTarget.All, preset);
    }

    public void callDeleteDistrict(string preset)
    {
        view.RPC("deleteDistrict", RpcTarget.All, preset);
    }

    public void callSetAssassinMarker(bool value)
    {
        view.RPC("setAssassinMarker", RpcTarget.All, value);
    }

    public bool getAssassinMarker()
    {
        return assassinMarker;
    }

    public void callSetMessage(string message)
    {
        view.RPC("setMessage", RpcTarget.All, message);
    }

    [PunRPC]
    private void increaseBalance(int amount)
    {
        balance += amount;
        if (view.IsMine)
        {
            var csm = GameObject.FindGameObjectWithTag("CSM").GetComponent<CharacterScreenManager>();
            csm.setBalance(balance);
        }
    }

    public int getBalance()
    {
        return balance;
    }

    [PunRPC]
    private void setCharacter(string preset)
    {
        character = preset;
    }

    [PunRPC]
    private void addDistrict(string preset)
    {
        districts.Add(preset);
    }

    [PunRPC]
    private void buildDistrict(string preset)
    {
        buildedDistricts.Add(preset);
    }

    [PunRPC]
    private void deleteDistrict(string preset)
    {
        districts.Remove(preset);
    }

    [PunRPC]
    private void setAssassinMarker(bool value)
    {
        assassinMarker = value;
    }

    [PunRPC]
    private void setMessage(string message)
    {
        var csm = GameObject.FindGameObjectWithTag("CSM").GetComponent<CharacterScreenManager>();
        if (view.IsMine)
        {
            csm.setMessage(message);
        }
    }

    public void callAskToResetDistricts()
    {
        view.RPC("askToResetDistricts", RpcTarget.All);

    }

    [PunRPC]
    private void askToResetDistricts()
    {
        if (view.IsMine)
        {
            var csm = GameObject.FindGameObjectWithTag("CSM").GetComponent<CharacterScreenManager>();
            csm.resetMasterDistricts();
        }
    }

    public void callAskToSetDistricts(List<string> newDistricts)
    {
        view.RPC("askToSetDistricts", RpcTarget.All, newDistricts);
    }

    private void askToSetDistricts(List<string> newDistricts)
    {
        if (view.IsMine)
        {
            var csm = GameObject.FindGameObjectWithTag("CSM").GetComponent<CharacterScreenManager>();
            csm.setMasterDistricts(newDistricts);
        }
    }
}
