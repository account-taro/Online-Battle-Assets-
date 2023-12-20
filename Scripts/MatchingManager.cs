using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
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
    public static bool gameStaet2 = false;
    public Transform scaffoldParent;
    public  List<Transform>  scaffolds;
    public GameManager gameManager;
    public GameObject leaderboard;

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
        PhotonNetwork.ConnectUsingSettings();
        //gameManager.GetComponent<GameManager>();
        for (int i = 0; i < scaffoldParent.childCount; i++)
        {
            scaffolds.Add(scaffoldParent.GetChild(i));
        }
        //btn = GameObject.Find("StartBtn");
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
        // ルームの参加人数を10人に設定する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        gameManager.gameObject.SetActive(true);
        PhotonNetwork.NickName = "Player" + PhotonNetwork.LocalPlayer.ActorNumber;
        int id = photonView.OwnerActorNr;
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

    // 他プレイヤーがルームから退出した時に呼ばれるコールバック
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Debug.Log($"{otherPlayer.NickName}が退出しました");
        if(gameStaet)
        {
            gameManager.reducePlayers();
        }
    }



    public void CreatePlayer()
    {
        photonView.RPC(nameof(_CreatePlayer), RpcTarget.All);
    }

    [PunRPC]
    void _CreatePlayer()
    {
        StartCoroutine(CreatePlayerCoroutine());
    }


    IEnumerator CreatePlayerCoroutine()
    {
        gameStaet = true;
        leaderboard.SetActive(false);
        btn.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        gameManager.situationText.text = "3";
        yield return new WaitForSeconds(1);
        gameManager.situationText.text = "2";
        yield return new WaitForSeconds(1);
        gameManager.situationText.text = "1";
        yield return new WaitForSeconds(1);
        gameManager.situationText.text = "START";
        yield return new WaitForSeconds(1);
        gameManager.situationText.text = "";
        gameStaet2 = true;
        position = scaffolds[Random.Range(0, 23)].position + new Vector3(0, 1);
        //if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        //{
        //    position = new Vector3(-17, 3);
        //}
        //else
        //if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        //{
        //    position = new Vector3(18, 2);
        //}
        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        gameManager.roomProperty["numberOfPeople"] = PhotonNetwork.CurrentRoom.PlayerCount;
        //gameManager.playerProperty["playerName"] = PhotonNetwork.LocalPlayer.NickName + "(" + PhotonNetwork.LocalPlayer.ActorNumber + ")";
        //Debug.Log(PhotonNetwork.LocalPlayer.NickName + "(" + PhotonNetwork.LocalPlayer.ActorNumber + ")");
        PhotonNetwork.LocalPlayer.SetCustomProperties(gameManager.playerProperty);
        PhotonNetwork.CurrentRoom.SetCustomProperties(gameManager.roomProperty);
    }
}