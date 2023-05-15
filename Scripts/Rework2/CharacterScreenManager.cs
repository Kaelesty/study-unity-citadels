using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScreenManager : MonoBehaviour
{
    PhotonView view;

    public GameObject characterMenu;
    public GameObject characterMenuHeader;
    public GameObject gamestateIndicator;
    public GameObject endTurnButton;

    public GameObject charPrefab;
    public GameObject distPrefab;
    public GameObject camera;

    

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        endTurnButton.SetActive(false);
        characterMenuHeader.SetActive(false);
        characterMenu.transform.LeanScale(new Vector3(0.06f, 0.12f), 0);
        characterMenu.transform.LeanRotateAroundLocal(new Vector3(0, 0, 180), 360, 3).setEaseInCubic().setLoopPingPong();
    }

    private GameObject getMasterPlayer()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PlayerRework>().checkView())
            {
                return player;
            }
        }
        throw new MissingComponentException();
    }

    public bool turn(int id, int stage)
    {
        var master = getMasterPlayer().GetComponent<PlayerRework>();
        if (master.checkView() && master.id == id) {

            gamestateIndicator.GetComponent<Text>().text = "“вой ход!";
            if (stage == 0)
            {
                LeanTween.cancel(characterMenu);
                characterMenu.transform.rotation = Quaternion.identity;
                characterMenu.transform.LeanScale(new Vector3(1f, 1f), 1).setEaseInCubic().setOnComplete(
                    delegate ()
                    {
                        characterMenuHeader.SetActive(true);
                        createCharacterVariants();
                    }
                    );
            }
            else
            {
                createResourcesVariants();
            }
            return true;
        }
        else
        {
            gamestateIndicator.GetComponent<Text>().text = "ќжидание хода...";

        }
        return false;
    }

    private void createCharacterVariants()
    {
        var deckManager = GameObject.FindGameObjectWithTag("DeckManager").GetComponent<DeckManager>();
        var deck = deckManager.getDeck();
        int i = 0;
        var len = deck.Length * 200 - 20;
        foreach (string character in deck)
        {
            var card = InstantiateCharCard(character);
            card.transform.LeanMove(new Vector2(2000, 900), 0);
            card.transform.LeanMove(new Vector3(-(len / 2) + 200 * i + 60 + 2000, 0, 200), 3).setEaseInOutCubic();
            i++;
        }
    }

    private void createResourcesVariants()
    {
        var deckManager = GameObject.FindGameObjectWithTag("DeckManager").GetComponent<DeckManager>();
        var len = 3 * 200 - 20;
        for (int i = 0; i<3 ;i++) 
        {
            var card = InstantiateDistrictCard(deckManager.takeDistrict());
            card.transform.LeanMove(new Vector2(2000, 900), 0);
            card.transform.LeanMove(new Vector3(-(len / 2) + 200 * i + 60 + 2200, 0, 200), 3).setEaseInOutCubic();
            i++;
        }
    }

    public void endTurnButtonClick()
    {
        endTurnButton.SetActive(false);
        var tm = GameObject.FindGameObjectWithTag("TurnManager");
        tm.GetComponent<TurnManager>().callEndTurn();
    }

    public GameObject InstantiateCharCard(string presetName)
    {
        GameObject charCard = Instantiate(charPrefab, transform.position, Quaternion.identity);

        var cardScript = charCard.GetComponent<CharacterCard>();
        cardScript.canvas.GetComponent<Canvas>().worldCamera = camera.GetComponent<Camera>();


        cardScript.loadPreset(presetName);
        return charCard;
    }

    public GameObject InstantiateDistrictCard(string presetName)
    {
        GameObject charCard = Instantiate(distPrefab, transform.position, Quaternion.identity);

        var cardScript = charCard.GetComponent<DistrictCard>();
        cardScript.canvas.GetComponent<Canvas>().worldCamera = camera.GetComponent<Camera>();
        cardScript.preset = presetName;
        cardScript.loadPreset(presetName);
        return charCard;
    }

    public void cardTaken(GameObject cardObject)
    {
        bool movingStarted = false;
        cardObject.tag = "PlayerCharacterCard";

        var master = getMasterPlayer();
        master.GetComponent<PlayerRework>().callSetCharacter(cardObject.GetComponent<CharacterCard>().preset);

        var deckManager = GameObject.FindGameObjectWithTag("DeckManager").GetComponent<DeckManager>();
        deckManager.callDeleteFromDeck(cardObject.GetComponent<CharacterCard>().preset);

        cardObject.GetComponent<CharacterCard>().takeButton.SetActive(false);

        foreach (GameObject card in GameObject.FindGameObjectsWithTag("CharacterCard"))
        {
            card.transform.LeanScale(new Vector3(0, 0, 0), 2).setEaseInOutCubic().setOnComplete(
            delegate()
            {
                Destroy(card);
                if (!movingStarted)
                {
                    movingStarted = true;
                    cardObject.transform.LeanScale(new Vector3(2.2f, 2.2f, 1), 1).setEaseInOutCubic().setOnComplete(
                        delegate ()
                        {
                            cardObject.transform.LeanMoveLocal(new Vector3(1600, 0, cardObject.transform.position.z), 1).setEaseInOutCubic();
                            var tm = GameObject.FindGameObjectWithTag("TurnManager");
                            tm.GetComponent<TurnManager>().callEndTurn();
                        });
                }
            });
        }
    }
}
