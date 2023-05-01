using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Game_Controller controller;
    public string preset;


    public GameObject canvas;

    public string description;
    public string color;

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
        // TODO : ���������� � CharacterCard.loadPreset...
    }

    public void activateBuilding()
    {
        buildButton.SetActive(true);
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