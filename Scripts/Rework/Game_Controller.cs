using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Security.Cryptography;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;

public class Game_Controller : MonoBehaviour
{
    private System.Random random = new System.Random();

    //SpawnManager
    public GameObject[] positions;

    // TurnManager
    private int freePosition = 0;
    private int currentTurn = 1;
    private string gameState = "CharacterSelecting";

    //DeckManager
    private string[] characters = { "Assassin", "Thief", "Bishop", "Magican", "Architect", "Merchant", "Warlord" };
    public string[] deck;
    public string laidOutPrivate;
    public string laidOutPublic;
    public GameObject charPrefab;



    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }



    public GameObject getFreePosition()
    {
        freePosition++;
        positions[freePosition].SetActive(true);
        return positions[freePosition];
    }
    public GameObject getZeroPosition()
    {
        positions[0].SetActive(true);
        return positions[0];
    }

    public int checkTurn()
    {
        return currentTurn;
    }
    public void nextTurn()
    {
        currentTurn++;
        if (currentTurn > PhotonNetwork.CurrentRoom.PlayerCount)
        {
            currentTurn = 1;
        }
    }
    public void generateDeck()
    {
        deck = characters;
        deck = deck.OrderBy(x => random.Next()).ToArray();

        laidOutPrivate = deck[0];
        laidOutPublic = deck[1];

        deck = deck.Skip(1).ToArray();
        deck = deck.Skip(1).ToArray();

        deck = deck.Append("King").ToArray();

        deck = deck.OrderBy(x => random.Next()).ToArray();
    }
    public string[] getDeck()
    {
        return deck;
    }

    public void deleteCard(string NameCard)
    {
        deck = deck.Where(e => e != NameCard).ToArray();
    }

    public void Start()
    {
        generateDeck();
    }

    public void renderDeck()
    {
        for (int i = 0; i < deck.Length; i++)
        {
            var charCard = InstantiateCharCard(deck[i]);
            charCard.transform.position += new Vector3(140 * i - ((140 * deck.Length / 2) - 50 - 20), 0, 0);

        }
    }
    private GameObject InstantiateCharCard(string presetName)
    {
        GameObject charCard = Instantiate(charPrefab, transform.position, Quaternion.identity);

        // этот кусок кода удалить после написания метода CharacterCard.loadPreset
        var cardScript = charCard.GetComponent<CharacterCard>();
        var text = cardScript.nameField.GetComponent<Text>();
        text.text = presetName;

        cardScript.loadPreset(presetName); // не написано
        return charCard;
    }

    public Dictionary<string, string> getCharNames()
    {
        return new Dictionary<string, string>() {
            { "Assassin", "Ассассин"},
            { "Thief", "Вор"},
            { "Bishop", "Епископ"},
            { "Magican", "Чародей"},
            { "Architect", "Зодчий"},
            { "Merchant", "Купец"},
            { "Warlord", "Кондотьер"},
            { "King", "Король"},
        };
    }

    public Dictionary<string, string> getCharDesc()
    {
        return new Dictionary<string, string>() {
            { "Assassin", "выводит любого персонажа из игры на один раунд"},
            { "Thief", "способен украсть золото у другого персонажа (кроме Ассасина и его жертвы)"},
            { "Bishop", "получает защиту зданий от разрушения в этот раунд и «церковную десятину» — доход со своих кварталов синего цвета."},
            { "Magican", "даёт возможность получить новые карты из колоды в обмен на свои, либо обменять все имеющиеся у себя в руке карты на все карты, имеющиеся в руке у другого игрока."},
            { "Architect", "автоматически получает дополнительные два квартала из колоды после совершения действия (то есть после взятия денег или карт кварталов перед постройкой) и может построить вплоть до трёх кварталов (остальные персонажи строят только один квартал в ход)."},
            { "Merchant", "автоматически получает дополнительный денежный бонус в 1 монету и доход со своих кварталов зелёного цвета."},
            { "Warlord", "получает доход с военных (красных) кварталов. В конце своего хода может разрушить здание любого игрока (кроме Епископа) за плату, на 1 монету меньшую, чем стоимость постройки квартала. Кварталы стоимостью 1 можно разрушать бесплатно."},
            { "King", "получает дополнительный доход со своих кварталов королевского (жёлтого) цвета и позволяет владельцу первым совершать выбор персонажа в следующем раунде."},
        };
    }

    public string getGameState()
    {
        return gameState;
    }
}
