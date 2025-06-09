using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SimpleLauncher : MonoBehaviourPunCallbacks
{
    public GameObject formPrefab;

    [SerializeField] private GameObject gameCam;
    [SerializeField] private GameObject roomCam;
    [SerializeField] private GameObject nameUI;
    [SerializeField] private GameObject connectingUI;
    
    private string _nickName = "unnamed";

    public void ChangeNickname(string _name)
    {
        _nickName = _name;
    }

    public void JoinRoomButtonPressed()
    {
        PhotonNetwork.ConnectUsingSettings();
        
        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }
    
    void Start()
    {
        Screen.SetResolution(800, 600, false);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("We are in the lobby");

        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room.");

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int formIndex = (playerCount - 1) % 3;

        Vector3 spawnPosition = new Vector3(Random.Range(-5,5), 0, Random.Range(-5,5));
        roomCam.SetActive(false);
        gameCam.SetActive(true);
        GameObject player = PhotonNetwork.Instantiate(formPrefab.name, spawnPosition, Quaternion.identity);

        if (player.GetComponent<PhotonView>().IsMine)
        {
            CameraFollowPlayer cameraFollow = FindFirstObjectByType<CameraFollowPlayer>();
            if (cameraFollow != null)
            {
                cameraFollow.SetFollow(player.transform);
            }
        }

        Player formHandler = player.GetComponent<Player>();
        formHandler.photonView.RPC("SetInitialForm", RpcTarget.AllBuffered, formIndex);
        formHandler.photonView.RPC("SetNickname", RpcTarget.All, _nickName);
        
        PhotonNetwork.LocalPlayer.NickName = _nickName;
    }
}