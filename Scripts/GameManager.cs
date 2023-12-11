using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public ExitGames.Client.Photon.Hashtable roomProperty;
    void Start()
    {
        roomProperty = new ExitGames.Client.Photon.Hashtable();
        roomProperty["playnum"] = PhotonNetwork.CurrentRoom.PlayerCount;
    }

    void Update()
    {
        
    }
}
