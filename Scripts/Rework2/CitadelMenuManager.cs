using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitadelMenuManager : MonoBehaviour
{
    int builtCounter = 0;

    public bool debugAllowNegativeBalance = false;
    public bool debugUnlimitedBuilding = false;

    private bool building = false;
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

    public void distCardBuild(GameObject cardObject)
    {
        var csm = GameObject.FindGameObjectWithTag("CSM").GetComponent<CharacterScreenManager>();
        var master = csm.getMasterPlayer().GetComponent<PlayerRework>();
        if (cardObject.GetComponent<DistrictCard>().price > master.getBalance() && !debugAllowNegativeBalance) 
        {
            cardObject.GetComponent<DistrictCard>().shake();
            return;
        }
        if (building)
        {
            return;
        }
        building = true;
        master.callBuildDistrict(cardObject.GetComponent<DistrictCard>().preset);
        master.callDeleteDistrict(cardObject.GetComponent<DistrictCard>().preset);
        master.callIncreaseBalance(-cardObject.GetComponent<DistrictCard>().price);
        cardObject.GetComponent<DistrictCard>().buildButton.SetActive(false);
        builtCounter++;
        if (((builtCounter == 1 && master.character != "Architect") || (builtCounter == 3)) && !debugUnlimitedBuilding)
        {
            foreach (GameObject card in GameObject.FindGameObjectsWithTag("PlayerDistrictCard"))
            {
                card.GetComponent<DistrictCard>().buildButton.SetActive(false);
            }
        }
        

        cardObject.tag = "PlayerBuiltDistrict";
        cardObject.transform.LeanScale(new Vector3(1.6f, 1.6f, 1f), 1).setEaseInOutCubic().setOnComplete(
            delegate ()
            {
                int x = 3250 + (master.buildedDistricts.Count - 1) * 210;
                int y = 100;
                cardObject.transform.LeanMove(new Vector3(x, y, cardObject.transform.position.z), 1).setEaseInOutCubic().setOnComplete(
                    delegate () 
                    {
                        cardObject.transform.LeanScale(new Vector3(2.0f, 2.0f, 1), 1).setOnComplete(
                            delegate ()
                            {
                                building = false;
                            });
                    });
            });

    }
}
