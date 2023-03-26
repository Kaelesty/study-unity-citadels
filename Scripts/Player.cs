using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks
{
    PhotonView view;
    public int positionID;
    public int networkID;
    public GameObject Camera;
    public GameObject Cursor;
    public GameObject Body;
    public GameObject UI;
    public Player scriptPlayer;
    public Button endButton;

    private TurnManager turnManager;
    
    private void Awake(){
        view = GetComponent<PhotonView>();

        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();

        networkID = PhotonNetwork.LocalPlayer.ActorNumber;

        positionID = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().takeID();
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            (transform.eulerAngles.z + 60) * positionID
        );

        if (!view.IsMine)
        {
            Camera.SetActive(false);
            scriptPlayer.enabled = false;
            Cursor.SetActive(false);
        } else
        {
            Body.SetActive(false);
        }


        endButton.GetComponent<Button>().onClick.AddListener(endTurn);
        UI.SetActive(false);


    }

    void Start()
    {
        

    }

    public void switchActivePlayerID()
    {

    }


    // Update is called once per frame
    void Update()
    {
        UI.SetActive(turnManager.getActivePlayerID() == networkID);
        
    }

    public void endTurn()
    {
        turnManager.switchActivePlayerID();
    }
}
