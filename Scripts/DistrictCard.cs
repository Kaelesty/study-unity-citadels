using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistrictCard : MonoBehaviour
{
    public GameObject nameField;
    public GameObject takeButton;
    public GameObject imageSlot;

    public Player owner;
    public CharacterCard script;
    public GameObject cardObject;

    public Game_Controller controller;
    public string preset;


    public GameObject canvas;

    public string description;
    public string color;


    public void OnTaken()
    {
        //Debug.Log("onTaken");
        controller.addToDistrictDeck(preset);
        owner.addToDistricts(preset);
        controller.nextTurn();
    }

    public void loadPreset(string presetName)
    {
        // TODO : аналогично с CharacterCard.loadPreset...
    }
}
