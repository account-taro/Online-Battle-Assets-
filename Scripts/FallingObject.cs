using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviourPunCallbacks
{
    SpriteRenderer spriteRenderer;
    private void Start()
    {
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
        if(MatchingManager.gameStaet2)
        {
            transform.Translate(0, -0.1f, 0);
        }

        if(GameManager.gameEnd)
        {
            Destroy(this.gameObject);
        }
        //Test2 test2 = new Test2(2,"ttt");
        //test2.TestFunc();
    }
}

class Test2:GroundFall
{
    int a;
    string b;

    public Test2(int _a, string _b)
    {
        this.a = _a;
        this.b = _b;
        
    }

    public void TestFunc()
    {
        Debug.Log(a + b);
    }
}