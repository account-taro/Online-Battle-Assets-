using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GroundFall: MonoBehaviourPunCallbacks
{
    [SerializeField]
    float fallTime;
    Rigidbody2D rb;
    BoxCollider2D collider;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        rb.WakeUp();
        if(fallTime > 3)
        {
            photonView.RPC(nameof(Fall), RpcTarget.All);
        }
        if(GameManager.gameEnd)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    [PunRPC]
    void Fall()
    {
        //Destroy(this.gameObject);
        rb.bodyType = RigidbodyType2D.Dynamic;
        collider.isTrigger = true;   
    }

    [PunRPC]
    void Count()
    {
        if(MatchingManager.gameStaet)
        {
            fallTime += Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "UnderFoot" && !GameManager.gameEnd)
        {
            photonView.RPC(nameof(Count), RpcTarget.All);
        }

        if (collision.gameObject.tag == "OutSide")
        {
            Destroy(this.gameObject);
        }
    }
}
