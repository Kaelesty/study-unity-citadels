using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistrictCard : MonoBehaviour
{
    public GameObject nameField;
    public GameObject takeButton;
    public GameObject buildButton;
    public GameObject buildedButton;
    public GameObject imageSlot;

    public Player owner;
    public CharacterCard script;
    public GameObject cardObject;
    public GameObject descField;

    public Game_Controller controller;
    public string preset;


    public GameObject canvas;

    public string description;
    public string color;
    public int price;

    public bool isBuilded = false;


    private Dictionary<string, string> distName = new Dictionary<string, string>() {
            {"Tavern", "Таверна"},
            {"Market", "Рынок"},
            {"Trading Post", "Лавка"},
            {"Docks", "Порт"},
            {"Harbor", "Гавань"},
            {"Town Hall", "Ратуша"},
            {"Temple", "Храм"},
            {"Church", "Церковь"},
            {"Monastery", "Монастырь"},
            {"Cathedral", "Собор"},
            {"Watchtower", "Дозорная башня"},
            {"Prison", "Тюрьма"},
            {"Battlefield", "Марсово поле"},
            {"Fortress", "Крепость"},
            {"Manor", "Поместье"},
            {"Castle", "Замок"},
            {"Palace", "Дворец"},
            {"Haunted City", "Город Призраков"},
            {"Keep", "Форт"},
            {"Laboratory", "Лаборатория"},
            {"Smithy", "Кузня"},
            {"Observatory", "Обсерватория"},
            {"Graveyard", "Кладбище"},
            {"School of Magic", "Школа Магии"},
            {"Library", "Библиотека"},
            {"Great Wall", "Великая Стена"},
            {"University", "Университет"},
            {"Dragon Gate", "Врата Дракона"},
        };

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

    private Dictionary<string, int> distPrice = new Dictionary<string, int>() {
            {"Tavern", 1},
            {"Market", 2},
            {"Trading Post", 2},
            {"Docks", 3},
            {"Harbor", 4},
            {"Town Hall", 5},
            {"Temple", 1},
            {"Church", 2},
            {"Monastery", 3},
            {"Cathedral", 5},
            {"Watchtower", 1},
            {"Prison", 2},
            {"Battlefield", 3},
            {"Fortress", 5},
            {"Manor", 3},
            {"Castle", 4},
            {"Palace", 5},
            {"Haunted City", 2},
            {"Keep", 3},
            {"Laboratory", 5},
            {"Smithy", 5},
            {"Observatory", 5},
            {"Graveyard", 5},
            {"School of Magic", 6},
            {"Library", 6},
            {"Great Wall", 6},
            {"University", 8},
            {"Dragon Gate", 8},
        };


    public void OnTaken()
    {
        //Debug.Log("onTaken");
        var csm = GameObject.FindGameObjectWithTag("CSM");
        csm.distCardTaken(preset);
    }

    public void loadPreset(string presetName)
    {
        preset = presetName;
        
        if (distColor.ContainsKey(presetName))
        {
            color = distColor[presetName];
        }
        if (distPrice.ContainsKey(presetName))
        {
            price = distPrice[presetName];
        }
    }

    public void activateBuilding(bool activity)
    {
        buildButton.SetActive(activity);
    }

    public void onBuild()
    {
        buildButton.SetActive(false);
        buildedButton.SetActive(true);
        isBuilded = true;
        owner.buildDistrict(preset);
        owner.renderDistricts(true);
        owner.controller.callNextTurn();
    }
}
