using UnityEngine.SceneManagement;
using Photon.Pun;

public class PunConnect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
