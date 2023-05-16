using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public GameObject allowNegativeBalanceIndicator;
    public GameObject unlimitedBuildingIndicator;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            enableAllowNegativeBalance();
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            enableUnlimitedBuilding();
        }
    }

    private void enableAllowNegativeBalance()
    {
        var cmm = GameObject.FindGameObjectWithTag("CMM").GetComponent<CitadelMenuManager>();
        cmm.debugAllowNegativeBalance = true;
        allowNegativeBalanceIndicator.SetActive(true);
    }

    private void enableUnlimitedBuilding()
    {
        var cmm = GameObject.FindGameObjectWithTag("CMM").GetComponent<CitadelMenuManager>();
        cmm.debugUnlimitedBuilding = true;
        unlimitedBuildingIndicator.SetActive(true);
    }
}
