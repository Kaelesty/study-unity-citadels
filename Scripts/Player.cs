using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks
{
    PhotonView view;
    public int id;
    public GameObject Camera;
    public GameObject Cursor;
    public GameObject Body;
    public Player scriptPlayer;
    
    private void Awake(){
        view = GetComponent<PhotonView>();
        SpawnManager spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        id = spawnManager.takeID();
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            (transform.eulerAngles.z + 60)*id
        );

        if (!view.IsMine) {
            Cursor.SetActive(false);
            Camera.SetActive(false);
            scriptPlayer.enabled = false;
            
        }
        else {
            Body.SetActive(false);
        }
        
    }

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
