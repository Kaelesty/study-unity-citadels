using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviourPunCallbacks
{
    PhotonView view;
    public int[] queue = null;

    public GameObject endTurnButton;

    public int stage = 0;
    // 0 - выбор персонажей, 1 - основной
    // (Относительно GameRework) 1 - объединение всех Major этапов

    public int currentQueueIndex = 0;

    private string[] characters = { "Assassin", "Thief", "Magician", "King", "Bishop", "Merchant", "Architect", "Warlord" };

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    public void callInit()
    {
        view.RPC("nit", RpcTarget.All);
    }

    [PunRPC]
    public void init()
    {
        callRefillQueueByPlayerID();
        var csm = GameObject.FindGameObjectWithTag("CSM");
        if (csm.GetComponent<CharacterScreenManager>().turn(queue[0], stage))
        {
            // endTurnButton.SetActive(true);
        }
    }

    public void callEndTurn()
    {
        view.RPC("endTurn", RpcTarget.All);
    }

    [PunRPC]
    private void endTurn()
    {
        currentQueueIndex++;
        var csm = GameObject.FindGameObjectWithTag("CSM");
        var dm = GameObject.FindGameObjectWithTag("DeckManager");
        if (currentQueueIndex >= queue.Length)
        {
            currentQueueIndex = 0;
            if (stage == 0)
            {
                stage = 1;
                refillQueueByCharacters();
                csm.GetComponent<CharacterScreenManager>().turn(queue[0], stage);
            }
            else
            {
                stage = 0;
                dm.GetComponent<DeckManager>().callGenerateDeck();
                refillQueueByPlayerID(); // заменить на Seating, когда он будет готов
                csm.GetComponent<CharacterScreenManager>().resetCharacterAndTurn(queue[0], stage);
            }
            
        }
        else
        {
            csm.GetComponent<CharacterScreenManager>().turn(queue[currentQueueIndex], stage);
        }
    }

    public void callRefillQueueByPlayerID()
    {
        view.RPC("refillQueueByPlayerID", RpcTarget.All);
    }

    public void callRefillQueueByCharacters()
    {
        view.RPC("refillQueueByCharacters", RpcTarget.All);
    }

    public void callRefillQueueBySeating()
    {
        view.RPC("refillQueueBySeating", RpcTarget.All);
    }

    [PunRPC]
    private void refillQueueByPlayerID()
    {
        queue = new int[PhotonNetwork.CurrentRoom.PlayerCount];
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount;i++)
        {
            queue[i] = i + 1;
        }
    }

    [PunRPC]
    private void refillQueueByCharacters()
    {
        queue = new int[PhotonNetwork.CurrentRoom.PlayerCount];
        Dictionary<string, int> players = new Dictionary<string, int>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players[player.GetComponent<PlayerRework>().character] = player.GetComponent<PlayerRework>().id;
        }

        int i = 0;
        foreach (string character in characters)
        {
            if (players.ContainsKey(character)) {
                queue[i] = players[character];
                i++;
            }
        }
    }

    [PunRPC]
    private void refillQueueBySeating()
    {
        // TODO: refillQueueSeating
        // Заполнить очередь по "рассадке" - первый игрок-король, далее id + 1 относительно него и так далее
        // Пример:
        // [Персонаж-ID]
        // Епископ - 1
        // Купец - 2
        // Король - 3
        // Маг - 4
        // При такой "рассадке" Должна получиться очередь [3, 4, 1, 2]
        // id-шники смотреть - player.

        queue = new int[PhotonNetwork.CurrentRoom.PlayerCount];
        Dictionary<string, int> players = new Dictionary<string, int>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players[player.GetComponent<PlayerRework>().character] = player.GetComponent<PlayerRework>().id;
        }

        // в словаре players сейчас ключи-персонажи, значения-id игроков
    }
}
