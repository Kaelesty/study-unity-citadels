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

    public GameObject charPrefab;

    public GameObject Camera;
    public GameObject Cursor;
    public GameObject Body;
    public GameObject UI;
    public GameObject Indicator;

    public Player scriptPlayer;
    public Button endButton;

    private TurnManager turnManager;
    private DeckController deckController;


    private bool flag_deckTaken;
    private string[] deck;
    
    private void Awake(){
        view = GetComponent<PhotonView>();

        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        deckController = GameObject.Find("DeckManager").GetComponent<DeckController>();

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
        Indicator.SetActive(false);

        endButton.GetComponent<Button>().onClick.AddListener(endTurn);
        UI.SetActive(false);

        flag_deckTaken = false;
    }

    void Update()
    {
        if (turnManager.getActivePlayerID() == networkID)
        {
            UI.SetActive(true);
            view.RPC("enableIndicator", RpcTarget.All);
            if (!flag_deckTaken)
            {
                flag_deckTaken = true;
                deck = deckController.getDeck();

                for (int i=0;i<deck.Length;i++)
                {
                    GameObject charCard = Instantiate(charPrefab, transform.position, Quaternion.identity);
                    charCard.transform.position += new Vector3(140*i - ((140*deck.Length/2) - 50 - 20), 0, 0);
                    var text = charCard.GetComponent<CharacterCard>().nameField.GetComponent<Text>();
                    text.text = deck[i];
                }
            }
        }
        else
        {
            UI.SetActive(false);
            view.RPC("disableIndicator", RpcTarget.All);
        }
        

    }

    [PunRPC]
    void disableIndicator()
    {
        Indicator.SetActive(false);
    }

    [PunRPC]
    void enableIndicator()
    {
        Indicator.SetActive(true);
    }

    public void endTurn()
    {

        turnManager.switchActivePlayerID();
    }
}
