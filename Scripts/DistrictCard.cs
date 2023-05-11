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


    public void OnTaken()
    {
        //Debug.Log("onTaken");
        controller.addToDistrictDeck(preset);
        owner.addToDistricts(preset);
        controller.callNextTurn();
    }

    public void loadPreset(string presetName)
    {
        // TODO : аналогично с CharacterCard.loadPreset...
        var distName = controller.getDistName();
        var distColor = controller.getDistColor();
        var distPrice = controller.getDistPrice();
        var distDesc = controller.getDistDesc();

        preset = presetName;
        
        if (distColor.ContainsKey(presetName))
        {
            color = distColor[presetName];
        }
        if (distPrice.ContainsKey(presetName))
        {
            price = distPrice[presetName];
        }
        if (distDesc.ContainsKey(presetName))
        {
            description = distDesc[presetName];
        }
        if (distName.ContainsKey(presetName))
        {
            // цена временно выводится в поле названия
            nameField.GetComponent<Text>().text = distName[presetName] + " " + price.ToString();
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
