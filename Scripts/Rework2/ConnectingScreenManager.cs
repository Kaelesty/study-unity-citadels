using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingScreenManager : MonoBehaviourPunCallbacks
{
    PhotonView view;

    public GameObject screenManager;

    public GameObject awaitingMenu;
    public GameObject gameBeginButton;


    public GameObject nicknameMenu;
    public GameObject nicknameInput;

    private string[] errorMessages = { "Нужно ввести имя", "^_^ имя", 
    "Чтобы ввести имя, нужно...", "Без имени не может начаться игра эта"};
    private int errorIndex = 0;
    private int currentAwaitingLabel = 0;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    public void setNickname()
    {
        var nickname = nicknameInput.GetComponent<InputField>().text;

        if (nickname.Length > 0 ) 
        {
            var gameOwnerFlag = false;
            nicknameMenu.SetActive(false);
            // TODO: Проверка на дублирование ника
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetComponent<PlayerRework>().checkView())
                {
                    player.GetComponent<PlayerRework>().callSetNickname(nickname);
                    gameOwnerFlag = player.GetComponent<PlayerRework>().id == 1;
                    view.RPC("addNicknameLabel", RpcTarget.All, nickname);
                    break;
                }
            }
            awaitingMenu.SetActive(true);
            if (!gameOwnerFlag)
            {
                gameBeginButton.SetActive(false);
                
            }

        }
        else
        {
            errorIndex++;
            if (errorIndex == errorMessages.Length)
            {
                errorIndex = 0;
            }
            nicknameInput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = errorMessages[errorIndex];
        }
    }

    [PunRPC]
    public void addNicknameLabel(string nickname)
    {
        var awaitingState = awaitingMenu.active;
        awaitingMenu.SetActive(true);
        GameObject label = GameObject.FindGameObjectsWithTag("AwaitingPlayerLabel")[currentAwaitingLabel];
        currentAwaitingLabel++;
        label.GetComponent<Text>().text = nickname;
        label.SetActive(true);
        awaitingMenu.SetActive(awaitingState);
    }

    public void callGameBegin()
    {
        view.RPC("gameBegin", RpcTarget.All);
    }

    [PunRPC]
    private void gameBegin()
    {
        var sm = screenManager.GetComponent<ScreenManager>();
        sm.switchScreen("CharacterSelecting");

        var tm = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
        tm.init();
    }
}
