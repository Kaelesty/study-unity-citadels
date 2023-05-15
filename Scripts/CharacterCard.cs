using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{
    public GameObject nameField;
    public GameObject takeButton;
    public GameObject imageSlot;
    public CharacterCard script;
    public GameObject cardObject;

    public string preset;


    public GameObject canvas;

    public string description;
    public string color;

    Dictionary<string, string> charNames = new Dictionary<string, string>() {
            { "Assassin", "Ассассин"},
            { "Thief", "Вор"},
            { "Bishop", "Епископ"},
            { "Magician", "Чародей"},
            { "Architect", "Зодчий"},
            { "Merchant", "Купец"},
            { "Warlord", "Кондотьер"},
            { "King", "Король"},
        };


    public void OnTaken()
    {
        // Debug.Log("onTaken");
        var csm = GameObject.FindGameObjectWithTag("CSM").GetComponent<CharacterScreenManager>();
        csm.cardTaken(cardObject);
    }

    public void loadPreset(string presetName)
    {
        preset = presetName;
        if (charNames.ContainsKey(presetName))
        {
            nameField.GetComponent<Text>().text = charNames[presetName];
        }
        imageSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Character" + presetName);
        switch (presetName)
        {
            case "King":
                color = "yellow";
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
