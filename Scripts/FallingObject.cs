using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviourPunCallbacks
{
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        Debug.Log(PhotonNetwork.IsMasterClient);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OutSide")
        {
            spriteRenderer.enabled = false;
            this.gameObject.transform.position = new Vector2(Random.Range(-20f, 20f), Random.Range(40f, 20f));
        }
        if(collision.gameObject.name=="StartLine")
        {
            spriteRenderer.enabled = true;
        }
    }

    private void Update()
    {
        if(MatchingManager.gameStaet)
        {
            transform.Translate(0, -0.1f, 0);
        }
    }
}
