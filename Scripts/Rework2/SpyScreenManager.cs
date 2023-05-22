using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SpyScreenManager : MonoBehaviour
{

    List<string> players = new List<string>();
    private int prevDistrictsLength = -1;

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
        }
    }

    private void Update()
    {   
        var csm = GameObject.FindGameObjectWithTag("CSM").GetComponent<CharacterScreenManager>();
        var master = csm.getMasterPlayer().GetComponent<PlayerRework>();

        
        var previousCharacter = GameObject.FindGameObjectWithTag("SpyMenuCharacter");
        if (previousCharacter != null)
        {
            Destroy(previousCharacter);
        }

        List<PlayerRework> targets = new List<PlayerRework>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            targets.Add(player.GetComponent<PlayerRework>());
        }
        var dropdown = targetSelector.GetComponent<Dropdown>();
        string targetNickname;
        try
        {
            targetNickname = dropdown.options[dropdown.value].text;
        }
        catch {
            return;
        }
        PlayerRework target = null;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].nickname == targetNickname)
            {
                target = targets[i];
                break;
            }
        }
        if (target == null)
        {
            return;
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
        cardChar.transform.LeanMove(new Vector3(1300, -1200, 200), 0);
        cardChar.transform.LeanScale(new Vector3(2.75f, 2.75f, 1f), 0);
        cardChar.tag = "SpyMenuCharacter";
        cardChar.GetComponent<CharacterCard>().takeButton.SetActive(false);

        if (target.buildedDistricts.Count == prevDistrictsLength)
        {
            return;
        }
        foreach (var card in GameObject.FindGameObjectsWithTag("SpyMenuDistrict"))
        {
            if (card != null)
            {
                Destroy(card);
            }
        }
        prevDistrictsLength = target.buildedDistricts.Count;

        for (int i = 0; i < target.buildedDistricts.Count; i++)
        {
            presetDist = target.buildedDistricts[i];
            var cardDist = csm.InstantiateDistrictCard(presetDist);
            cardDist.transform.LeanMove(new Vector3(1750 + i*shift, -1135, 200), 0);
            cardDist.transform.LeanScale(new Vector3(1.6f, 1.6f, 1f), 0);
            cardDist.tag = "SpyMenuDistrict";
            cardDist.GetComponent<DistrictCard>().takeButton.SetActive(false);
            if (master.character == "Warlord" && csm.checkOurTurn())
            {
                cardDist.GetComponent<DistrictCard>().destroyButton.SetActive(true);
            }
        }
    }

    public void WarlordDestroy(GameObject card)
    {
        var csm = GameObject.FindGameObjectWithTag("CSM").GetComponent<CharacterScreenManager>();
        var master = csm.getMasterPlayer().GetComponent<PlayerRework>();

        if (master.getBalance() < card.GetComponent<DistrictCard>().price + 1)
        {
            card.GetComponent<DistrictCard>().shake();
            return;
        }
        master.callIncreaseBalance(-(card.GetComponent<DistrictCard>().price + 1));

        var dropdown = targetSelector.GetComponent<Dropdown>();
        string targetNickname;
        targetNickname = dropdown.options[dropdown.value].text;

        PlayerRework target = null;
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PlayerRework>().nickname == targetNickname)
            {
                target = player.GetComponent<PlayerRework>();
            }
        }
        target.callDestroyDistrict(card.GetComponent<DistrictCard>().preset);
        target.callAskToUpdateCitadel();
    }
}
