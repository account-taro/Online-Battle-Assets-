using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class MatchingManager : MonoBehaviourPunCallbacks
{
    Vector3 position;
    public TextMeshProUGUI situationText;
    Player[] players;
    public GameObject btn;
    public static bool gameStaet = false;
    public Transform scaffoldParent;
    public  List<Transform>  scaffolds;
    ExitGames.Client.Photon.Hashtable hashtable;

    private void Update()
    {
        if (!gameStaet)
        {
            players = PhotonNetwork.PlayerList;
            if (players.Length >= 2 && PhotonNetwork.IsMasterClient)
            {
                btn.SetActive(true);
            }
        }
    }
    private void Start()
    {
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.NickName = "Player";
        PhotonNetwork.ConnectUsingSettings();
        for (int i = 0; i < scaffoldParent.childCount; i++)
        {
            scaffolds.Add(scaffoldParent.GetChild(i));
        }
        hashtable = new ExitGames.Client.Photon.Hashtable();    
        hashtable["Score"] = 100000000000000000;
        hashtable["Message"] = "こんにちは";
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        Debug.Log(hashtable["Score"]);
    }



    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // ランダムなルームに参加する
        PhotonNetwork.JoinRandomRoom();
    }

    // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ルームの参加人数を2人に設定する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }

        // ルームが満員になったら、以降そのルームへの参加を不許可にする
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }



    public void CreatePlayer()
    {
        photonView.RPC(nameof(_CreatePlayer), RpcTarget.All);
    }

    [PunRPC]
    void _CreatePlayer()
    {
        position = scaffolds[Random.Range(0, 23)].position+new Vector3(0,1);
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            position = new Vector3(-17, 3);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            position = new Vector3(18, 2);
        }
        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        situationText.text = "";
        gameStaet = true;
        btn.SetActive(false);
    }

}