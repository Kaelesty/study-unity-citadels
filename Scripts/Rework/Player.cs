using Photon.Pun;

using UnityEngine;

public class Player : MonoBehaviourPunCallbacks
{
    PhotonView view;
    private Game_Controller controller;
    public int id;
    public GameObject position;

    public GameObject character;

    private bool characterSelected = false;

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
                        break;
                    case "Coming soon...":
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

    public void cardSelected(string preset)
    {
        Debug.Log("cardSelected");
        view.RPC("setCharacter", RpcTarget.All, preset);
    }

    [PunRPC]
    private void setCharacter(string preset)
    {
        Debug.Log("setCharacter");
        Debug.Log(id);
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
}
