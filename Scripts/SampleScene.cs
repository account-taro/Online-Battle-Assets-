using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

// MonoBehaviourPunCallbacks���p�����āAPUN�̃R�[���o�b�N���󂯎���悤�ɂ���
public class SampleScene : MonoBehaviourPunCallbacks
{
    Vector3 position;
    public TextMeshProUGUI situationText;
    private void Start()
    {
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.NickName = "Player";
        PhotonNetwork.ConnectUsingSettings();
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
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        // �����_���ȍ��W�Ɏ��g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
        if(PhotonNetwork.LocalPlayer.ActorNumber==1)
        {
            position = new Vector3(-17,3);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            position = new Vector3(18, 2);
        }

            PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        situationText.text = "";
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


}