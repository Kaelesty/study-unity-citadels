using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryMenuManager : MonoBehaviour
{
    public GameObject message;
    public void endgame()
    {
        var csm = GameObject.FindGameObjectWithTag("CSM");
        List<PlayerRework> players = new List<PlayerRework>();
        string result = "";
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player.GetComponent<PlayerRework>());
        }

        // Тут нужно заполнить строку result

        message.GetComponent<Text>().text = result;
    }
}
