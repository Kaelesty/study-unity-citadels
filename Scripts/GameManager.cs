using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public InputField inputField;
    
    public void createRoom() {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;
        PhotonNetwork.CreateRoom(inputField.text, roomOptions);
    }

    public void joinRoom() {
        PhotonNetwork.JoinRoom(inputField.text);
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("GameRework");
    }
}
