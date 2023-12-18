using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderFoot : MonoBehaviour
{
    AvatarController controller;
    void Start()
    {
        controller =transform.parent.GetComponent<AvatarController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag== "Ground" || collision.gameObject.tag == "Player")
        {
            controller.isGround = true;
            if(collision.gameObject.tag == "Player")
            {
                AvatarController target = collision.gameObject.GetComponent<AvatarController>();
                target.photonView.RPC(nameof(target.ChangeDamageAnimation), RpcTarget.All);
                target.wait = 1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            controller.isGround = false;
        }
    }
}
