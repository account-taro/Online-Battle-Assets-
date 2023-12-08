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
        if(collision.gameObject.tag== "Ground")
        {
            controller.isGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            controller.isGround = false;
        }
    }
}
