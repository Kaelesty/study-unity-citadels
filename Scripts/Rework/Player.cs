using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks
{
    PhotonView view;
    private Game_Controller controller;
    public int id;
    public GameObject position;

    public int money;

    public GameObject character;
    public string[] districts;

    private bool characterSelected = false;
    private bool districtsTaken = false;
    private bool districtsRendered = false;
    private bool resourcesRendered = false;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        controller = GameObject.Find("Controller").GetComponent<Game_Controller>();

        if (view.IsMine)
        {
            id = PhotonNetwork.LocalPlayer.ActorNumber;
            position = controller.getZeroPosition();
        }
        else
        {
            id = -1;
            position = controller.getFreePosition();
        }
        position.GetComponent<Game_Position>().owner = this;
        transform.position = new Vector3(0, 0, 0);

    }

    private void Update()
    {
        if (id != -1)
        {
            view.RPC("idSync", RpcTarget.Others, id);
        }
        if (controller.checkTurn() == id) {
            position.GetComponent<Game_Position>().Activate();
            if (view.IsMine)
            {
                switch (controller.getGameState())
                {
                    case "CharacterSelecting":
                        controller.renderDeck();
                        if (!districtsTaken)
                        {
                            districtsTaken=true;
                            for (int i = 0;i<4;i++)
                            {
                                districts = districts.Append(controller.takeRandomDistrict()).ToArray();
                            }
                        }
                        renderDistricts();
                        break;
                    case "Major: Resources":
                        if (!resourcesRendered) {
                            controller.renderResourcesUI(true);
                            resourcesRendered=true;
                        }
                        break;
                }
            }
        }
        else
        {
            position.GetComponent<Game_Position>().Unactivate();
        }
    }

    [PunRPC]
    private void idSync(int nuID)
    {
        id = nuID;
    }

    public void addMoney(int amount)
    {
        controller.updateMoneyIndicator(amount);
        view.RPC("addMoneySync", RpcTarget.All, amount);
    }

    [PunRPC]
    public void addMoneySync(int amount)
    {
        money += amount;
    }

    public void cardSelected(string preset)
    {
        
        view.RPC("setCharacter", RpcTarget.All, preset);
    }

    [PunRPC]
    private void setCharacter(string preset)
    {
        character = controller.InstantiateCharCard(preset);
        character.tag = "PlayerCharacterCard";
        character.GetComponent<CharacterCard>().takeButton.SetActive(false);
        characterSelected = true;
        if (view.IsMine)
        {
            character.transform.position = new Vector3(-870, -418, 0);
        }
        else
        {
            character.transform.position = position.transform.position + new Vector3(0, -150, 0);
        }

    }

    private void renderDistricts()
    {
        if (districtsRendered)
        {
            return;
        }
        //Debug.Log("RenderDistricts");
        foreach (var i in GameObject.FindGameObjectsWithTag("PlayerDistrictCard"))
        {
            Destroy(i);
        }

        districtsRendered = true;
        //Debug.Log(districts.Length);
        for (int i = 0;i< districts.Length; i++)
        {
            var district = controller.InstantiateDistrictCard(districts[i]);
            district.GetComponent<DistrictCard>().takeButton.SetActive(false);
            district.tag = "PlayerDistrictCard";
            district.transform.position = new Vector3(860 - 140 * i - 60, -418, 0);


        }
    }

    public void addToDistricts(string district)
    {
        //Debug.Log("AddToDistricts: " + id.ToString() + " // " + district);
        view.RPC("AddToDistrictsSync", RpcTarget.All, district);
        renderDistricts();
    }

    [PunRPC]
    private void AddToDistrictsSync(string district)
    {
        districts = districts.Append(district).ToArray();
        districtsRendered = false;
    }
}
