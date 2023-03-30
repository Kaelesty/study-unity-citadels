using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterCard : MonoBehaviour
{
    public GameObject nameField;
    public GameObject takeButton;
    public GameObject imageSlot;

    public Player owner;
    public CharacterCard script;
    public GameObject cardObject;

    public GameObject canvas;

    public string description;
    public string color;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnTaken()
    {

        owner.addCharacterCard(cardObject);
    }

    public void loadPreset(string presetName)
    {
        /*
        TO-DO 
        Функция принимает название пресета (персонажа), и подгружает из файла/словаря/... :

        Имя         (нужно записать в nameField)
        Описание    (нужно сохранить в переменную description) (временно, пока нет элемента интерфейса для этого)
        Картинку    (нужно установить в imageSlot)
        Цвет(тип)   (нужно записать в переменную color)

        */
    }
}
