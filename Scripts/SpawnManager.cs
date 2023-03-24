using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    public GameObject player;
    public int currId = 1;

    private void Awake() {
        var players = GameObject.FindGameObjectsWithTag("Player");
        PhotonNetwork.Instantiate(player.name, new Vector2(0, 0), Quaternion.Euler(0, 0, 0));
    }

    public int takeID() {
        currId += 1;
        return currId - 1;
    }
}
