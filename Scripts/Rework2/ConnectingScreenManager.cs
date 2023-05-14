using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingScreenManager : MonoBehaviour
{
    public GameObject awaitingMenu;
    public GameObject nicknameMenu;

    public GameObject nicknameInput;

    private string[] errorMessages = { "Нужно ввести имя", "^_^ имя", 
    "Чтобы ввести имя, нужно...", "Без имени не может начаться игра эта"};
    private int errorIndex = 0;

    public void setNickname()
    {
        var nickname = nicknameInput.GetComponent<InputField>().text;

        if (nickname.Length > 0 ) 
        {
            nicknameMenu.SetActive(false);
            awaitingMenu.SetActive(true);
        }
        else
        {
            errorIndex++;
            if (errorIndex == errorMessages.Length)
            {
                errorIndex = 0;
            }
            nicknameInput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = errorMessages[errorIndex];
        }
    }
}
