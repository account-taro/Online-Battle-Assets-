using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// MonoBehaviourPunCallbacks���p�����āAPUN�̃R�[���o�b�N���󂯎���悤�ɂ���
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
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.NickName = "Player";
        PhotonNetwork.ConnectUsingSettings();
        for (int i = 0; i < scaffoldParent.childCount; i++)
        {
            scaffolds.Add(scaffoldParent.GetChild(i));
        }
        hashtable = new ExitGames.Client.Photon.Hashtable();    
        hashtable["Score"] = 100000000000000000;
        hashtable["Message"] = "����ɂ���";
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        Debug.Log(hashtable["Score"]);
    }



    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        // �����_���ȃ��[���ɎQ������
        PhotonNetwork.JoinRandomRoom();
    }

    // �����_���ŎQ���ł��郋�[�������݂��Ȃ��Ȃ�A�V�K�Ń��[�����쐬����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���[���̎Q���l����2�l�ɐݒ肷��
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }

        // ���[���������ɂȂ�����A�ȍ~���̃��[���ւ̎Q����s���ɂ���
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