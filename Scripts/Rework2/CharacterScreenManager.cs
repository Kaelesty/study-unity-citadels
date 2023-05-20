using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScreenManager : MonoBehaviour
{
    PhotonView view;

    public GameObject characterMenu;
    public GameObject characterMenuHeader;
    public GameObject resourcesMenuHeader;
    public GameObject gamestateIndicator;
    public GameObject endTurnButton;

    public GameObject charPrefab;
    public GameObject distPrefab;
    public GameObject camera;

    private Dictionary<string, string> distColor = new Dictionary<string, string>() {
            {"Tavern", "green"},
            {"Market", "green"},
            {"Trading Post", "green"},
            {"Docks", "green"},
            {"Harbor", "green"},
            {"Town Hall", "green"},
            {"Temple", "blue"},
            {"Church", "blue"},
            {"Monastery", "blue"},
            {"Cathedral", "blue"},
            {"Watchtower", "red"},
            {"Prison", "red"},
            {"Battlefield", "red"},
            {"Fortress", "red"},
            {"Manor", "yellow"},
            {"Castle", "yellow"},
            {"Palace", "yellow"},
            {"Haunted City", "purple"},
            {"Keep", "purple"},
            {"Laboratory", "purple"},
            {"Smithy", "purple"},
            {"Observatory", "purple"},
            {"Graveyard", "purple"},
            {"School of Magic", "purple"},
            {"Library", "purple"},
            {"Great Wall", "purple"},
            {"University", "purple"},
            {"Dragon Gate", "purple"},
        };
    private Dictionary<string, string> charNames = new Dictionary<string, string>() {
            { "Assassin", "Ассассин"},
            { "Thief", "Вор"},
            { "Bishop", "Епископ"},
            { "Magician", "Чародей"},
            { "Architect", "Зодчий"},
            { "Merchant", "Купец"},
            { "Warlord", "Кондотьер"},
            { "King", "Король"},
        };




    private void Awake()
    {
        view = GetComponent<PhotonView>();
        characterMenuHeader.SetActive(false);
        characterMenu.transform.LeanScale(new Vector3(0.06f, 0.12f), 0);
        characterMenu.transform.LeanRotateAroundLocal(new Vector3(0, 0, 180), 360, 3).setEaseInCubic().setLoopPingPong();

        var resourcesMenu = GameObject.FindGameObjectWithTag("ResourcesMenu");
        resourcesMenu.LeanScale(new Vector3(0f, 0f, 1f), 0);
    }

    public void init()
    {
        var dm = GameObject.FindGameObjectWithTag("DeckManager").GetComponent<DeckManager>();
        var cmm = GameObject.FindGameObjectWithTag("CMM").GetComponent<CitadelMenuManager>();
        var master = getMasterPlayer().GetComponent<PlayerRework>();
        for (int i = 0; i < 4; i++)
        {
            var card = dm.takeDistrict();
            master.callAddDistrict(card);
        }
        cmm.updatePlayerDistricts(master.districts);
    }

    public GameObject getMasterPlayer()
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

    public bool turn(int id, int stage, bool callFromCharReset=false)
    {
        var master = getMasterPlayer().GetComponent<PlayerRework>();
        if (callFromCharReset)
        {
            var resourcesMenu = GameObject.FindGameObjectWithTag("ResourcesMenu");
            resourcesMenu.transform.LeanScale(new Vector3(0f, 0f, 1), 1).setEaseOutCubic();
            resourcesMenuHeader.GetComponent<Text>().text = "Выберите карту";
            resourcesMenu.transform.LeanScale(new Vector3(0f, 0f, 1), 0);

            var description = GameObject.FindGameObjectWithTag("Description");
            description.transform.LeanScale(new Vector3(0f, 0f, 1), 0);
        }
        if (master.checkView() && master.id == id) {
            if (master.getAssassinMarker())
            {
                master.callSetAssassinMarker(false);
                endTurnButtonClick();
                return false;
            }
            GameObject.FindGameObjectWithTag("MessageUI").LeanScale(new Vector3(0f, 0f, 1f), 0).setEaseInOutCubic();
            GameObject.FindGameObjectWithTag("Message").LeanScale(new Vector3(0f, 0f, 1f), 0).setEaseInOutCubic();


            gamestateIndicator.GetComponent<Text>().text = "Твой ход!";
            if (stage == 0)
            {
                foreach (GameObject card in GameObject.FindGameObjectsWithTag("PlayerDistrictCard"))
                {
                    card.GetComponent<DistrictCard>().buildButton.SetActive(false);
                }
                foreach (GameObject card in GameObject.FindGameObjectsWithTag("DistrictCard"))
                {
                    card.LeanScale(new Vector3(0f, 0f, 0f), 0.1f).setEaseOutCubic().setOnComplete(
                        delegate ()
                        {
                            Destroy(card);
                        }); 
                }
                LeanTween.cancel(characterMenu);
                characterMenu.transform.rotation = Quaternion.identity;
                endTurnButton.SetActive(false);
                characterMenu.transform.LeanMove(new Vector3(2000, 0, characterMenu.transform.position.z), 1).setEaseInOutCubic().setOnComplete(
                    delegate ()
                    {
                        characterMenu.transform.LeanScale(new Vector3(1f, 1f), 1).setEaseInCubic().setOnComplete(
                            delegate ()
                            {
                                characterMenuHeader.SetActive(true);
                                createCharacterVariants();
                            }
                            );
                    });
            }
            else
            {
                createResourcesVariants();
                endTurnButton.SetActive(true);

                foreach (GameObject card in GameObject.FindGameObjectsWithTag("PlayerDistrictCard"))
                {
                    card.GetComponent<DistrictCard>().buildButton.SetActive(true);
                }
            }
            return true;
        }
        else
        {
            gamestateIndicator.GetComponent<Text>().text = "Ожидание хода...";
            endTurnButton.SetActive(false);
            foreach (GameObject card in GameObject.FindGameObjectsWithTag("DistrictCard"))
            {
                card.transform.LeanScale(new Vector3(0f, 0f, 0f), 1).setEaseOutCubic().setOnComplete(
                    delegate ()
                    {
                        Destroy(card);
                    });
            }
            var resourcesMenu = GameObject.FindGameObjectWithTag("ResourcesMenu");
            if (resourcesMenu.transform.localScale.x > 0f)
            {
                resourcesMenu.transform.LeanScale(new Vector3(0.11f, 0.15f, 1), 1);
                resourcesMenu.transform.LeanMoveLocal(new Vector3(170, 0, 0), 0);
                resourcesMenu.transform.LeanRotateAroundLocal(new Vector3(0, 0, 180), 360, 3).setEaseInCubic().setLoopPingPong();
            }
            if (view.IsMine && callFromCharReset)
            {
                Debug.Log("callFromCharReset");
                characterMenu.transform.LeanMove(new Vector3(2000, 0, characterMenu.transform.position.z), 1).setEaseInOutCubic();
                characterMenuHeader.SetActive(false);
                characterMenu.transform.LeanScale(new Vector3(0.06f, 0.12f), 0);
                characterMenu.transform.LeanRotateAroundLocal(new Vector3(0, 0, 180), 360, 3).setEaseInCubic().setLoopPingPong();
            }
        }
        return false;
    }

    public void resetCharacterAndTurn(int id, int stage)
    {
        var character = GameObject.FindGameObjectWithTag("PlayerCharacterCard");
        character.transform.LeanScale(new Vector3(0f, 0f, 0f), 1).setEaseOutCubic().setOnComplete(
            delegate ()
            {
                Destroy(character);
                var master = getMasterPlayer().GetComponent<PlayerRework>();
                master.callSetCharacter("");
                characterMenu.transform.LeanMove(new Vector3(2000, 0, characterMenu.transform.position.z), 1).setEaseInOutCubic();
                characterMenuHeader.SetActive(false);
                characterMenu.transform.LeanScale(new Vector3(0.06f, 0.12f), 0);
                characterMenu.transform.LeanRotateAroundLocal(new Vector3(0, 0, 180), 360, 3).setEaseInCubic().setLoopPingPong();
                turn(id, stage, true);
            });
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

        var resourcesMenu = GameObject.FindGameObjectWithTag("ResourcesMenu");
        LeanTween.cancel(resourcesMenu);
        resourcesMenu.transform.rotation = Quaternion.identity;
        resourcesMenu.transform.LeanScale(new Vector3(1f, 1f), 1).setEaseInCubic().setOnComplete(
            delegate ()
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject card;
                    if (i == 0)
                    {
                        card = InstantiateDistrictCard("Money");
                    }
                    else
                    {
                        card = InstantiateDistrictCard(deckManager.takeDistrict());
                    }
                    card.transform.LeanMove(new Vector2(2000, 900), 0);
                    card.transform.LeanMove(new Vector3(-(len / 2) + 200 * i + 60 + 2200, 0, 200), 2).setEaseInOutCubic();
                }
            }
            );
    }

    public void endTurnButtonClick()
    {
        if (getMasterPlayer().GetComponent<PlayerRework>().character == "Thief") {
            handleThiefSkill();
        }
        else if (getMasterPlayer().GetComponent<PlayerRework>().character == "Assassin")
            {
                handleAssassinSkill();
            }
        endTurnButton.SetActive(false);
        var tm = GameObject.FindGameObjectWithTag("TurnManager");
        tm.GetComponent<TurnManager>().callEndTurn();
        GameObject.FindGameObjectWithTag("SkillTargetSelector").LeanScale(new Vector3(0f, 0f, 1f), 0);
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
            characterMenuHeader.SetActive(false);
            GameObject characterMenu = GameObject.FindGameObjectWithTag("CharacterMenu");
            characterMenu.transform.LeanScale(new Vector3(0.3f, 0.85f, 1), 1).setEaseInOutCubic().setOnComplete(
                delegate ()
                {
                    characterMenu.transform.LeanMove(new Vector3(
                cardObject.transform.position.x,
                cardObject.transform.position.y,
                cardObject.transform.position.z), 1).setEaseInOutCubic().setOnComplete(
                delegate ()
                {
                    card.transform.LeanScale(new Vector3(0, 0, 0), 2).setEaseInOutCubic().setOnComplete(
                    delegate ()
                    {
                        Destroy(card);
                        if (!movingStarted)
                        {
                            movingStarted = true;
                            cardObject.transform.LeanScale(new Vector3(2.8f, 2.8f, 1), 1).setEaseInOutCubic().setOnComplete(
                                delegate ()
                                {
                                    cardObject.transform.LeanMoveLocal(new Vector3(1300, 0, cardObject.transform.position.z), 1).setEaseInOutCubic();
                                    characterMenu.transform.LeanMove(new Vector3(1300, 0, characterMenu.transform.position.z), 1).setEaseInOutCubic();
                                    var resourcesMenu = GameObject.FindGameObjectWithTag("ResourcesMenu");
                                    resourcesMenu.transform.LeanScale(new Vector3(0.11f, 0.15f, 1), 1);
                                    resourcesMenu.transform.LeanMoveLocal(new Vector3(170, 0, 0), 0);
                                    resourcesMenu.SetActive(true);
                                    resourcesMenu.transform.LeanRotateAroundLocal(new Vector3(0, 0, 180), 360, 3).setEaseInCubic().setLoopPingPong();
                                    var tm = GameObject.FindGameObjectWithTag("TurnManager");
                                    tm.GetComponent<TurnManager>().callEndTurn();
                                });
                        }
                    });
                });
                });
        }
    }

    public void distCardTaken(GameObject cardObject)
    {
        cardObject.tag = "PlayerDistrictCard";
        cardObject.GetComponent<DistrictCard>().activateBuilding(false);

        var cardObjectKickedOut = false;
        
        var master = getMasterPlayer().GetComponent<PlayerRework>();


        if (cardObject.GetComponent<DistrictCard>().preset != "Money")
        {
            master.callAddDistrict(cardObject.GetComponent<DistrictCard>().preset);
        }
        else
        {
            master.callIncreaseBalance(2);
        }

        var dm = GameObject.FindGameObjectWithTag("DeckManager").GetComponent<DeckManager>();
        foreach (GameObject card in GameObject.FindGameObjectsWithTag("DistrictCard"))
        {
            dm.callReturnDistrict(card.GetComponent<DistrictCard>().preset);
            card.transform.LeanMove(new Vector2(2000, 900), 1).setEaseInOutCubic().setOnComplete(
                delegate ()
                {
                    Destroy(card);
                    if (!cardObjectKickedOut)
                    {
                        cardObjectKickedOut = true;
                        cardObject.transform.LeanScale(new Vector3(2.3f, 2.3f, 1f), 1).setEaseInOutCubic().setOnComplete(
                        delegate ()
                        {
                            int x, y;
                            if (cardObject.GetComponent<DistrictCard>().preset != "Money")
                            {
                                y = -320;
                                x = 3200 + 140 * (master.districts.Count - 1);
                            }
                            else
                            {
                                y = 1000;
                                x = 2000;
                            }
                            cardObject.transform.LeanMoveLocal(
                                new Vector3(x, y, cardObject.transform.position.z), 2).setEaseInOutCubic().setOnComplete(
                                delegate()
                                {
                                    if (cardObject.GetComponent<DistrictCard>().preset != "Money")
                                    {
                                        cardObject.GetComponent<DistrictCard>().takeButton.SetActive(false);
                                        cardObject.GetComponent<DistrictCard>().buildButton.SetActive(true);
                                        cardObject.transform.LeanScale(new Vector3(1.2f, 1.2f, 1), 0);
                                    }
                                    else
                                    {
                                        Destroy(cardObject);
                                    }
                                    createSkillsMenu();
                                });
                        });
                    }
                });
        }
    }

    private void createSkillsMenu()
    {
        resourcesMenuHeader.SetActive(false);
        var resourcesMenu = GameObject.FindGameObjectWithTag("ResourcesMenu");
        resourcesMenu.transform.LeanScale(new Vector3(0.0f, 0.0f, 1), 1).setEaseInOutCubic().setOnComplete(
            delegate()
            {
                resourcesMenuHeader.SetActive(true);
                resourcesMenuHeader.GetComponent<Text>().text = "Способность";
                string text = "";
                var master = getMasterPlayer().GetComponent<PlayerRework>();
                int newBalance;
                DeckManager dm;
                GameObject selector;
                Dropdown dropdown;
                string[] options = { "--------", "Король", "Чародей", "Купец", "Зодчий", "Кондотьер", "Епископ"};
                switch (master.character)
                {
                    case "Merchant":
                        newBalance = 1;
                        foreach (var district in master.buildedDistricts)
                        {
                            if (distColor[district] == "green")
                            {
                                newBalance++;
                            }
                        }
                        master.callIncreaseBalance(newBalance);
                        text = "Пассивная способность:\n★ +1 золотой\n★ +1 золотой за каждый зеленый район";
                        break;
                    case "Bishop":
                        newBalance = 0;
                        foreach (var district in master.buildedDistricts)
                        {
                            if (distColor[district] == "blue")
                            {
                                newBalance++;
                            }
                        }
                        master.callIncreaseBalance(newBalance);
                        text = "Пассивная способность:\n★ +1 золотой за каждый синий район\n★ Ваши районы нельзя разрушить";
                        break;
                    case "Architect":
                        dm = GameObject.FindGameObjectWithTag("DeckManager").GetComponent<DeckManager>();
                        for (int i = 0; i < 2;i++)
                        {
                            var card = dm.takeDistrict();
                            master.callAddDistrict(card);
                            var cardObject = InstantiateDistrictCard(card);
                            cardObject.tag = "PlayerDistrictCard";
                            cardObject.GetComponent<DistrictCard>().takeButton.SetActive(false);
                            cardObject.GetComponent<DistrictCard>().buildButton.SetActive(true);
                            cardObject.transform.LeanScale(new Vector3(1.2f, 1.2f, 1), 0);
                            int y = -320;
                            int x = 3200 + 140 * (master.districts.Count - 1);
                            cardObject.transform.LeanMove(new Vector3(x, y, 1), 0);
                        }
                        text = "Пассивная способность:\n★ Можно построить до трех районов\n★ Получено 2 карты районов";
                        break;
                    case "King":
                        text = "Пассивная способность:\n★ Следующий ход начнется с вас";
                        break;
                    case "Thief":
                        text = "Активная способность:\n★ Выберите персонажа, все деньги которого\nперейдут к вам";
                        dm = GameObject.FindGameObjectWithTag("DeckManager").GetComponent<DeckManager>();
                        selector = GameObject.FindGameObjectWithTag("SkillTargetSelector");
                        selector.transform.LeanScale(new Vector3(1f, 1f, 1f), 1).setEaseInOutCubic();
                        dropdown = selector.GetComponent<Dropdown>();
                        dropdown.ClearOptions();
                        foreach (var item in options)
                        {
                            var option = new Dropdown.OptionData();
                            option.text = item;
                            dropdown.options.Add(option);
                        }
                        dropdown.value = 0;
                        break;
                    case "Assassin":
                        text = "Активная способность:\n★ Выберите персонажа, который пропустит этот ход";
                        dm = GameObject.FindGameObjectWithTag("DeckManager").GetComponent<DeckManager>();
                        selector = GameObject.FindGameObjectWithTag("SkillTargetSelector");
                        selector.transform.LeanScale(new Vector3(1f, 1f, 1f), 1).setEaseInOutCubic();
                        dropdown = selector.GetComponent<Dropdown>();
                        dropdown.ClearOptions();
                        options.Append("Вор");
                        foreach ( var item in options )
                        {
                            var option = new Dropdown.OptionData();
                            option.text = item;
                            dropdown.options.Add(option);
                        }
                        dropdown.value = 0;
                        break;
                    case "Warlord":
                        text = "Активная способность:\n★ Выберите игрока и его район, который будет разрушен\n(Меню шпионажа)";
                        break;
                    case "Magician":
                        text = "Активная способность:\n★ Выберите игрока, с которым вы обменяетесь картами районов\nИЛИ Сбросьте свои карты в колоду и\n получите столько же новых";
                        break;
                }
                var description = GameObject.FindGameObjectWithTag("Description");
                description.transform.LeanScale(new Vector3(1f, 1f, 1f), 0);
                description.GetComponent<Text>().text = text;

                resourcesMenu.transform.LeanScale(new Vector3(1, 1, 1), 1).setEaseInOutCubic();
            });
    }

    public void setBalance(int balance)
    {
        var balanceIndicator = GameObject.FindGameObjectWithTag("Balance");
        balanceIndicator.GetComponent<Text>().text = "Баланс: " + balance.ToString();
    }

    public PlayerRework getPlayerWithCharacter(string character)
    {
        PlayerRework result = null;
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PlayerRework>().character == character)
            {
                result = player.GetComponent<PlayerRework>();
                break;
            }
        }
        return result;
    }

    private void handleThiefSkill()
    {
        var master = getMasterPlayer().GetComponent<PlayerRework>();
        var dropdown =  GameObject.FindGameObjectWithTag("SkillTargetSelector").GetComponent<Dropdown>();
        var characterToRob = dropdown.options[dropdown.value].text;
        PlayerRework target = null;
        foreach (var value in charNames.Keys)
        {
            if (charNames[value] == characterToRob)
            {
                target = getPlayerWithCharacter(value);
            }
        }
        if (target != null)
        {
            int profit = target.getBalance();
            target.callIncreaseBalance(-profit);
            master.callIncreaseBalance(profit);
            target.callSetMessage("Вы были ограблены!");
        }
    }

    private void handleAssassinSkill()
    {
        var master = getMasterPlayer().GetComponent<PlayerRework>();
        var dropdown = GameObject.FindGameObjectWithTag("SkillTargetSelector").GetComponent<Dropdown>();
        var characterToKill = dropdown.options[dropdown.value].text;
        PlayerRework target = null;
        foreach (var value in charNames.Keys)
        {
            if (charNames[value] == characterToKill)
            {
                target = getPlayerWithCharacter(value);
            }
        }
        if (target != null)
        {
            target.callSetAssassinMarker(true);
            target.callSetMessage("Вы пропускаете ход");
        }
    }

    public void setMessage(string message)
    {
        var messageUI = GameObject.FindGameObjectWithTag("Message");
        GameObject.FindGameObjectWithTag("MessageUI").LeanScale(new Vector3(1f, 1f, 1f), 0).setEaseInOutCubic();
        messageUI.LeanScale(new Vector3(1f, 1f, 1f), 0).setEaseInOutCubic();
        messageUI.GetComponent<Text>().text = message;
    }
}
