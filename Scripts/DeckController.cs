using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;
using System.Security.Cryptography;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;

public class DeckController : MonoBehaviourPunCallbacks
{
    public string[] deck;

    private string[] characters = { "Assassin", "Thief", "Bishop", "Magican", "Architect", "Merchant", "Warlord" };

    private string laidOutPrivate;
    public string laidOutPublic;

    private System.Random random = new System.Random();

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

    public Dictionary<string, string> getCharNames() {
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
};
