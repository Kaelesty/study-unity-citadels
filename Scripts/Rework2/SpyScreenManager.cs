using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SpyScreenManager : MonoBehaviour
{

    List<string> players = new List<string>();

    public GameObject targetSelector;
    public void init()
    {
        var csm = GameObject.FindGameObjectWithTag("CSM").GetComponent<CharacterScreenManager>();
        var master = csm.getMasterPlayer().GetComponent<PlayerRework>();
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (master.nickname == player.GetComponent<PlayerRework>().nickname)
            {
                continue;
            }
            players.Add(player.GetComponent<PlayerRework>().nickname);
        }

        var dropdown = targetSelector.GetComponent<Dropdown>();
        dropdown.ClearOptions();
        for (int i = 0; i < players.Count; i++)
        {
            var option = new Dropdown.OptionData();
            option.text = players[i];
            dropdown.options.Add(option);
        }
        if (dropdown.options.Count > 0)
        {
            dropdown.value = 0;
            targetSelectorOnChange();
        }
    }

    public void targetSelectorOnChange()
    {
        var csm = GameObject.FindGameObjectWithTag("CSM").GetComponent<CharacterScreenManager>();
        var master = csm.getMasterPlayer().GetComponent<PlayerRework>();
        List<PlayerRework> targets = new List<PlayerRework>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            targets.Add(player.GetComponent<PlayerRework>());
        }
        var dropdown = targetSelector.GetComponent<Dropdown>();
        var targetNickname = dropdown.options[dropdown.value].text;
        PlayerRework target = null;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].nickname == targetNickname)
            {
                target = targets[i];
                break;
            }
        }

        var presetChar = "";
        var presetDist = "";
        var shift = 175;

        if (target.characterShown == true)
        {
            presetChar = target.character;
        }
        else
        {
            presetChar = "unknown";
        }

        var cardChar = csm.InstantiateCharCard(presetChar);
        cardChar.transform.LeanMove(new Vector3(2000, 0, cardChar.transform.position.z), 1);
        cardChar.tag = "SpyMenuCharacter";

        for (int i = 0; i < target.buildedDistricts.Count; i++)
        {
            presetDist = target.buildedDistricts[i];
            var cardDist = csm.InstantiateDistrictCard(presetDist);
            cardDist.transform.LeanMove(new Vector3(1800 + i*shift, 1150, cardChar.transform.position.z), 1);
            cardDist.tag = "SpyMenuDistricts";
        }
        // TODO : Экран шпионажа
        // В переменной targetNickname лежит НИКНЕЙМ игрока, за которым хотим шпионить
        // В переменной targets лежит список ОБЪЕКТОВ игроков
        // Нужно: пройтись по списку targets, найти игрока у которого nickname == targetNickname, записать его объект в target
        // Далее, если target.characterShown == true, создать карту персонажа с пресетом target.Character, 
        // иначе создать карту персонажа с пресетом "unknown"
        // (создать карту персонажа -> var card = csm.InstantiateCharCard(<пресет>))
        // передвинуть эту карту в нужное место на экране (В окошко "Персонаж")
        // Далее, пройтись по списку target.buildedDistricts (в нем лежат пресеты построенныхй районов)
        // для каждого из них создать карту района
        // (создать карту района -> var card = csm.InstantiateDistCard(<пресет>))
        // каждую из этих карт передвитуть на нужную позицию на экране (В окошко "Цитадель")
        // похвалить себя за то как все шикарно сделано
    }
}
