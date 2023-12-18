using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class AvatarController : MonoBehaviourPunCallbacks
    //,IPunObservable
{
    GameManager gameManager;
    Animator animator;
    Rigidbody2D rb;
    public GameObject underFoot;
    public bool isGround = false;
    GameObject mainCamera;
    public GameObject playerCamera;
    public float wait;
    private void Start()
    {
        if (!photonView.IsMine)
        {
            var camera = transform.Find("Camera").gameObject;
            camera.SetActive(false);
        }
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.SetBool("Beside", true);
        underFoot = transform.Find("UnderFoot").gameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainCamera = GameObject.Find("MainCamera"); 
        mainCamera.SetActive(false);
        //photonView.RPC(nameof(ChangeName), RpcTarget.All);
        //Debug.Log(photonView.Controller);
        //this.name =  photonView.Controller.ActorNumber.ToString();
    }

    //private const float MaxStamina = 6f;

    //[SerializeField]
    //private Image staminaBar = default;

    //private float currentStamina = MaxStamina;


    private void Update()
    {
        if (photonView.IsMine && !GameManager.gameEnd && wait<0)
        {
            photonView.RPC(nameof(ChangeWalkAnimation), RpcTarget.All);
            if (Input.GetAxis("Horizontal")!=0)
            {
                transform.Translate(6*Time.deltaTime * Input.GetAxis("Horizontal"), 0, 0);
                animator.SetFloat("Speed", 1);
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                animator.SetFloat("Speed", 1);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                animator.SetFloat("Speed", 1);
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGround)
            {
                rb.AddForce(Vector3.up * 400);
            }

            //if (Input.GetKeyDown(KeyCode.Return) && isGround)
            //{
            //    rb.AddForce(Vector3.right * 1000);
            //}
        }

        wait-= Time.deltaTime;
        // スタミナをゲージに反映する
        //staminaBar.fillAmount = currentStamina / MaxStamina;
        //Debug.Log(playerProperty["alive"]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="OutSide" && photonView.IsMine && !GameManager.gameEnd)
        {
            gameManager.Lost(playerCamera,this.gameObject);
        }

        if(collision.gameObject.tag=="FallingObject")
        {
            photonView.RPC(nameof(ChangeDamageAnimation), RpcTarget.All);
            wait = 1;
            //Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber+"が触れました");
        }
    }

    [PunRPC]
    public void ChangeDamageAnimation()
    {
        animator.SetBool("Damage", true);
        animator.SetBool("Beside", false);
    }

    [PunRPC]
    void ChangeWalkAnimation()
    {
        animator.SetBool("Damage", false);
        animator.SetBool("Beside", true);
    }

    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable playerProperty)
    //{
    //    foreach(var prop in playerProperty)
    //    {
    //        //Debug.Log($"{targetPlayer.NickName}{targetPlayer.ActorNumber}/{prop.Key}: {prop.Value}");
    //    }
    //}

    //[PunRPC]
    //void ChangeName()
    //{
    //    string playerName = (PhotonNetwork.LocalPlayer.CustomProperties["playerName"] is string value) ? value : "";
    //    this.name = playerName;
    //}


    //void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        // 自身のアバターのスタミナを送信する
    //        stream.SendNext(currentStamina);
    //    }
    //    else
    //    {
    //        // 他プレイヤーのアバターのスタミナを受信する
    //        currentStamina = (float)stream.ReceiveNext();
    //    }
    //}
}

class Test : AvatarController
{

}
