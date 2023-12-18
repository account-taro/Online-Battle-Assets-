using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Globalization;
using Photon.Pun.Demo.PunBasics;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Hashtable roomProperty;
    public Hashtable playerProperty;
    public GameObject mainCamera;
    public TextMeshProUGUI t;
    int numberOfPeople;
    bool alive;
    public TextMeshProUGUI situationText;
    public static bool gameEnd;
    GameObject winPlayer;
    string winPlayerName;


    void Start()
    {
        roomProperty = new Hashtable();
        roomProperty["numberofpeople"] = 0;
        roomProperty["winPlayerName"] = "";
        //roomProperty["winPlayer"] = null;
        playerProperty = new Hashtable();
        //playerProperty["playerName"] = PhotonNetwork.LocalPlayer.NickName + "(" + PhotonNetwork.LocalPlayer.ActorNumber+")";
        playerProperty["alive"] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperty);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperty);
        foreach (var e in roomProperty)
        {
            Debug.Log($"{e.Key}:{e.Value}");
        }

    }

    void Update()
    {
        int numberOfPeople = (PhotonNetwork.CurrentRoom.CustomProperties["numberOfPeople"] is int value) ? value : 0;
        //t.text = numberOfPeople.ToString();
        if (numberOfPeople == 1)
        {
            Win();
        }
    }

    public void reducePlayers()
    {
        int currentPlayers = (PhotonNetwork.CurrentRoom.CustomProperties["numberOfPeople"] is int value) ? value : 0;
        int newPlayers = currentPlayers - 1;
        roomProperty["numberOfPeople"] = newPlayers;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperty);
    }

    public void Win()
    {
        bool alive = (PhotonNetwork.LocalPlayer.CustomProperties["alive"] is bool value) ? value : true;
        if (alive)
        {
            situationText.text = "YOUER WIN!!";
            //roomProperty["winPlayer"] = GameObject.Find(PhotonNetwork.LocalPlayer.NickName + "/" + PhotonNetwork.LocalPlayer.ActorNumber);
            roomProperty["winPlayerName"] = PhotonNetwork.LocalPlayer.NickName;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperty);
            Debug.Log(roomProperty["winPlayerName"]);
            gameEnd = true;
        }
        else
        {
            StartCoroutine(WinCoroutine());
            IEnumerator WinCoroutine()
            {
                yield return new WaitForSeconds(1); // ìKêÿÇ»ë“Çøéûä‘Çê›íË
                string winPlayerName = (PhotonNetwork.CurrentRoom.CustomProperties["winPlayerName"] is string value2) ? value2 : "";
                situationText.text = "WINNER " + winPlayerName;
                Debug.Log(roomProperty["winPlayerName"]);
                gameEnd = true;
            }
        }
    }

    public void Lost(GameObject playerCamera, GameObject player)
    {
        playerCamera.SetActive(false);
        mainCamera.SetActive(true);
        situationText.text = "YOUER LOST...";
        playerProperty["alive"] = false;
        reducePlayers();
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperty);
        Destroy(player.gameObject);
    }

    //IEnumerable ShowWinPlayer()
    //{
    //    gameEnd = true;
    //    situationText.text = "";
    //    yield return new WaitForSeconds(1);
    //    string winPlayer = (PhotonNetwork.CurrentRoom.CustomProperties["winPlayer"] is string value) ? value : "";
    //    situationText.text = "WINNER " + winPlayer;
    //}
    //public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    //{
    //    foreach (var prop in propertiesThatChanged)
    //    {
    //        Debug.Log($"{prop.Key}:{prop.Value}");
    //    }
    //}
}




