using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitadelMenuManager : MonoBehaviour
{
    public void updatePlayerDistricts(List<string> districts)
    {
        int x0 = 3200;
        int y0 = -320;

        var csm = GameObject.FindGameObjectWithTag("CSM").GetComponent<CharacterScreenManager>();

        for (int i = 0; i < districts.Count; i++)
        {
            var card = csm.InstantiateDistrictCard(districts[i]);
            card.tag = "PlayerDistrictCard";
            card.transform.position = new Vector3(x0 + i * 140, y0, 200);
            card.GetComponent<DistrictCard>().takeButton.SetActive(false);
            card.GetComponent<DistrictCard>().buildButton.SetActive(false);
            card.transform.LeanScale(new Vector3(1.2f, 1.2f, 1), 0);
        }
    }
}
