using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    public GameObject player;

    private void Awake()
    {
        PhotonNetwork.Instantiate(player.name, new Vector2(0, 0), Quaternion.Euler(0, 0, 0));
    }
}