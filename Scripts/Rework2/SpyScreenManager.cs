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
        cardChar.transform.LeanMove(new Vector3(1300, -1200, cardChar.transform.position.z), 1);
        cardChar.tag = "SpyMenuCharacter";

        for (int i = 0; i < target.buildedDistricts.Count; i++)
        {
            presetDist = target.buildedDistricts[i];
            var cardDist = csm.InstantiateDistrictCard(presetDist);
            cardDist.transform.LeanMove(new Vector3(1750 + i*shift, -1135, cardChar.transform.position.z), 1);
            cardDist.tag = "SpyMenuDistricts";
        }
    }
}
