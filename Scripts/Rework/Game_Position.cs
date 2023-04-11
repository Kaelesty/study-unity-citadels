using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Position : MonoBehaviour
{
    public GameObject activeMarker;
    public GameObject unavtiveMarker;
    public Player owner;
    public void Activate()
    {
        activeMarker.SetActive(true);
        unavtiveMarker.SetActive(false);
    }
    public void Unactivate()
    {
        activeMarker.SetActive(false);
        unavtiveMarker.SetActive(true);
    }
}
