using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
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
        Debug.Log("onTaken");
        controller.characterSelected(preset);
        owner.cardSelected(preset);
    }

    public void loadPreset(string presetName)
    {
        preset = presetName;
        var charNames = controller.getCharNames();
        var charDesc = controller.getCharDesc();
        if (charNames.ContainsKey(presetName))
        {
            nameField.GetComponent<Text>().text = charNames[presetName];
        }
        if (charDesc.ContainsKey(presetName))
        {
            description = charDesc[presetName];
        }
        imageSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Character" + presetName);
        switch (presetName)
        {
            case "King":
                color = "gold";
                break;
            case "Warlord":
                color = "red";
                break;
            case "Merchant":
                color = "green";
                break;
            case "Bishop":
                color = "blue";
                break;
            default:
                color = "white";
                break;
        }
    }
}
