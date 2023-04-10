using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks
{
    PhotonView view;
    private Game_Controller controller;
    public int id;
    public GameObject position;

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
            switch (controller.getGameState())
            {
                case "CharacterSelecting":
                    controller.renderDeck();
                    break;
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
}
