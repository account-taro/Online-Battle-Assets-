using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OutSide : MonoBehaviourPunCallbacks
{    
    public GameManager gameManager;
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Avatar" && photonView.IsMine)
        {
            gameManager.reducePlayers();
        }
    }

}
