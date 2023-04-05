using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public DeckController deckController;

    public GameObject canvas;

    public string description;
    public string color;

    void Start()
    {
        deckController = GameObject.Find("DeckManager").GetComponent<DeckController>();
    }

    void Update()
    {
        
    }

    public void OnTaken()
    {

        owner.addCharacterCard(nameField.GetComponent<Text>().text);
    }

    public void loadPreset(string presetName)
    {
        /*
        TO-DO 
        Функция принимает название пресета (персонажа): "Assassin", "Thief", "Bishop", "Magican", "Architect", "Merchant", "Warlord"
        И заполняет поля в соответствии с нужным пресетом:
        Имя         (нужно записать в nameField) (Получается из словаря deckController.getCharNames())
        Описание    (нужно сохранить в переменную description) (временно, пока нет элемента интерфейса для этого) (Получается из словаря deckController.getCharDesc())
        Картинку    (нужно установить в imageSlot) (Заглушки картинок лежат в папке images)
        Цвет(тип)   (нужно записать в переменную color, временно, пока нет элемента интерфейса для этого) (Цвета персонажей как в игре, gold у короля, red у кондотьера, green у торгаша, blue у епископа, white у остальных)

        */
    }
}
