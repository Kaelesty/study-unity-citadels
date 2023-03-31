using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviourPunCallbacks
{
    PhotonView view;
    public int positionID; // id позиции игрока на столе, получаемый из SpawnManager (не синхронизируется)
    public int networkID; // id КЛИЕНТА в сети (почему то начинается с 1)

    public GameObject charPrefab;

    public GameObject Camera;
    public GameObject Cursor; // индикатор активного игрока (синий треугольник)
    public GameObject Body; // индикатор другого игрока (красный треугольник)
    public GameObject UI;
    public GameObject Indicator; // индикатор текущего хода (белый треугольник)

    public Player scriptPlayer;

    private TurnManager turnManager;
    private DeckController deckController;


    private bool flag_deckTaken; // кусок неиспользуемой логики, который страшно удалить
    private string[] deck; // названия карт декущей деки, получаемые из DeckController
    private GameObject characterCard; // карта персонажа на текущем ходу


    private void Awake() {
        view = GetComponent<PhotonView>();

        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        deckController = GameObject.Find("DeckManager").GetComponent<DeckController>();

        networkID = PhotonNetwork.LocalPlayer.ActorNumber;

        positionID = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().takeID();
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            (transform.eulerAngles.z + 60) * positionID
        );

        if (!view.IsMine)
        {
            Camera.SetActive(false);
            scriptPlayer.enabled = false;
            Cursor.SetActive(false);
        } else
        {
            Body.SetActive(false);
        }
        Indicator.SetActive(false);

        UI.SetActive(false);
        flag_deckTaken = false;
    }

    private GameObject InstantiateCharCard(string presetName)
    {
        GameObject charCard = Instantiate(charPrefab, transform.position, Quaternion.identity);
        
        var cardScript = charCard.GetComponent<CharacterCard>();

        // этот кусок кода удалить после написания метода CharacterCard.loadPreset
        var text = cardScript.nameField.GetComponent<Text>();
        cardScript.canvas.GetComponent<Canvas>().worldCamera = Camera.GetComponent<Camera>();
        cardScript.owner = this;
        text.text = presetName;
        //
        cardScript.loadPreset(presetName); // не написано
        return charCard;
    }

    void Update()
    {
        //
        // TurnManager синхронизирует параметр ActivePlayerID. она равна id клиента => наш ход
        if (turnManager.getActivePlayerID() == networkID)
        {
            UI.SetActive(true);
            view.RPC("enableIndicator", RpcTarget.All);
            if (!flag_deckTaken)
            {
                flag_deckTaken = true;
                deck = deckController.getDeck();

                for (int i = 0; i < deck.Length; i++)
                {
                    var charCard = InstantiateCharCard(deck[i]);
                    charCard.transform.position += new Vector3(140 * i - ((140 * deck.Length / 2) - 50 - 20), 0, 0);
                    
                }
            }
        }
        else
        {
            UI.SetActive(false);
            view.RPC("disableIndicator", RpcTarget.All);
        }


    }


    // вызывается из CharacterCard, сохраняет выбранную карту персонажа
    public void addCharacterCard(string presetName)
    {
        view.RPC("addCharacterCard_RPC", RpcTarget.All, presetName);
        deckController.deleteCard(presetName);
    }

    [PunRPC]
    void addCharacterCard_RPC(string presetName)
    {
        characterCard = InstantiateCharCard(presetName);
        Destroy(characterCard.GetComponent<CharacterCard>().takeButton);
        characterCard.tag = "PlayerCharacterCard";
        characterCard.transform.parent = transform;
        characterCard.transform.position = new Vector3(0, 0, 0);
        endTurn();
    }


    // методы используются для синхронизации индикаторов хода
    [PunRPC]
    void disableIndicator()
    {
        Indicator.SetActive(false);
    }

    [PunRPC]
    void enableIndicator()
    {
        Indicator.SetActive(true);
    }

    public void endTurn()
    {
        foreach (GameObject card in GameObject.FindGameObjectsWithTag("CharacterCard"))
        {
            Destroy(card);
        }
        turnManager.switchActivePlayerID();
    }
}
