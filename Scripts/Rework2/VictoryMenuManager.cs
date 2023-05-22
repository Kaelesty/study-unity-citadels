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
        var tm = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
        List<PlayerRework> players = new List<PlayerRework>();
        string result = "";
        string resultWinners = "";
        var points = 0;
        var colors = new List<string>();
        int maxPoints = 0;
        List<PlayerRework> winners = new List<PlayerRework>();
        var buildedDistricts = player.buildedDistricts;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player.GetComponent<PlayerRework>());
            foreach (var district in buildedDistricts)
            {
                var districtPrice = csm.distPrice[district];
                points += districtPrice;

                var color = csm.distColor[district];
                if (!colors.Contains(color))
                {
                    colors.Add(color);
                }
            }

            if ((colors.Count == 5) || (colors.Count == 4 && buildedDistricts.Contains("Haunted")))
            {
                points += 3;
            }

            if (buildedDistricts.Count == 8)
            {
                points += 2;
            }

            if (player.id == tm.getWinner())
            {
                points += 2;
            }

            if (buildedDistricts.Contains("University") || buildedDistricts.Contains("Dragon Gate"))
            {
                points += 2;
            }

            if (points > maxPoints)
            {
                maxPoints = points;
                winners.Clear();
                winners.Add(player);
            }

            else if (points == maxPoints)
            {
                winners.Add(player);
            }


        }

        result += $"{player.nickname}: {points}\n";

        message.GetComponent<Text>().text = result;

        if (winners.Count == 1)
        {
            resultWinners += $"Победил игрок {winners[0].nickname} с результатом {maxPoints} очков!\n";
        }

        else if (winners.Count > 1)
        {
            resultWinners += "Ничья! Выиграли следующие игроки:\n";
            foreach (var winner in winners)
            {
                resultWinners += $"{winner.nickname} с результатом {maxPoints} очков";
            }
        }
    }
}
