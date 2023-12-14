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
    AvatarController avatarController;
    public TextMeshProUGUI t;
    int numberOfPeople;
    bool alive;
    public TextMeshProUGUI situationText;

    void Start()
    {
        roomProperty = new Hashtable();
        roomProperty["numberofpeople"] = 0;
        playerProperty = new Hashtable();
        playerProperty["alive"] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperty);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperty);
    }

    void Update()
    {
        int numberOfPeople = (PhotonNetwork.CurrentRoom.CustomProperties["numberOfPeople"] is int value) ? value : 0;
        t.text = numberOfPeople.ToString();
        if(numberOfPeople==1)
        {
            Win();
        }
    }

    public void reducePlayers()
    {
        int currentPlayers = (PhotonNetwork.CurrentRoom.CustomProperties["numberOfPeople"] is int value) ? value : 0;
        int newPlayers = currentPlayers - 1;
        Debug.Log(newPlayers);
        roomProperty["numberOfPeople"] = newPlayers;
        Debug.Log(roomProperty["numberOfPeople"]);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperty);
    }

    public void Win()
    {
        bool alive = (PhotonNetwork.LocalPlayer.CustomProperties["alive"] is bool value) ? value : true;
        if (alive)
        {
            situationText.text = "YOUER WIN!!";
        }
    }

    //public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    //{
    //    foreach (var prop in propertiesThatChanged)
    //    {
    //        Debug.Log($"{prop.Key}:{prop.Value}");
    //    }
    //}
}




