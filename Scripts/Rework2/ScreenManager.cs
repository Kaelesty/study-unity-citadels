using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{

    public GameObject screenSelector;
    public GameObject camera;

    private Dictionary<string, int[]> screens = new Dictionary<string, int[]>();

    public void Start()
    {
        screens["Connecting"] = new int[2] { 0, 0 };
        screens["CharacterSelecting"] = new int[2] { 2000, 0 };

        var dropdown = screenSelector.GetComponent<Dropdown>();
        dropdown.ClearOptions();
        foreach (var screen in screens.Keys)
        {
            if (screen == "Connecting")
            {
                continue;
            }
            var option = new Dropdown.OptionData();
            option.text = screen;
            dropdown.options.Add(option);
        }
    }

    public void selectorSwitchScreen()
    {
        screenSelector.SetActive(true);
        var dropdown = screenSelector.GetComponent<Dropdown>();
        switchScreen(dropdown.options[dropdown.value].text);
    }

    public void switchScreen(string screenName)
    {
        var screenPosition = screens[screenName];
        camera.transform.LeanMoveLocal(new Vector3(screenPosition[0], screenPosition[1], (float)-325.4922), 2).setEaseInCubic().setOnComplete(
            delegate()
            {
                if (screenName == "CharacterSelecting") { enableSelector(); }
            });
    }

    public void enableSelector()
    {
        screenSelector.SetActive(true);
    }
}
