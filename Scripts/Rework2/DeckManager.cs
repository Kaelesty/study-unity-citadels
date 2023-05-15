using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviourPunCallbacks
{
    PhotonView view;
    private System.Random random = new System.Random();

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        callGenerateDeck();
        callGenerateDistDeck();
    }

    public string[] characters = { "Assassin", "Thief", "Magician", "Bishop", "Architect", "Merchant", "Warlord" };
    public string[] districts = { "Tavern", "Market", "Trading Post", "Docks", "Harbor", "Town Hall", "Temple", "Church", "Monastery", "Cathedral", "Watchtower", "Prison", "Battlefield", "Fortress", "Manor", "Castle", "Palace", "Haunted", "Keep", "Laboratory", "Smithy", "Graveyard", "Observatory", "School of Magic", "Library", "Great Wall", "University", "Dragon Gate" };

    public string[] deck;
    public string[] distDeck;

    public void callGenerateDeck()
    {
        deck = characters;
        deck = deck.OrderBy(x => random.Next()).ToArray();

        //laidOutPrivate = deck[0];
        //laidOutPublic = deck[1];

        deck = deck.Skip(1).ToArray();
        deck = deck.Skip(1).ToArray();

        deck = deck.Append("King").ToArray();

        deck = deck.OrderBy(x => random.Next()).ToArray();
        view.RPC("setDeck", RpcTarget.All, deck);
    }

    public void callGenerateDistDeck()
    {
        distDeck = districts;
        distDeck = deck.OrderBy(x => random.Next()).ToArray();
        view.RPC("setDistDeck", RpcTarget.All, distDeck);
    }

    [PunRPC]
    private void setDeck(string[] newDeck)
    {
        deck = newDeck;
    }

    [PunRPC]
    private void setDistDeck(string[] newDistDeck)
    {
        distDeck = newDistDeck;
    }
    public string[] getDeck()
    {
        return deck;
    }

    public void callDeleteFromDeck(string card)
    {
        view.RPC("deleteFromDeck", RpcTarget.All, card);
    }

    [PunRPC]
    private void deleteFromDeck(string card)
    {
        deck = deck.Where(e => e != card).ToArray();
    }

    public string takeDistrict()
    {
        string dist = distDeck[0];
        view.RPC("deleteFirstFromDistrictDeck", RpcTarget.All);
        return dist;
    }

    [PunRPC]
    private void deleteFirstFromDistrictDeck()
    {
        distDeck = distDeck.Skip(1).ToArray();
    }
}
