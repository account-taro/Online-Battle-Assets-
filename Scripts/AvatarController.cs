using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class AvatarController : MonoBehaviourPunCallbacks
    //,IPunObservable
{
    Animator animator;
    float horizintal;
    float vertintal;
    Rigidbody2D rb;
    public GameObject underFoot;
    public bool isGround = false;
    GameObject mainCamera;
    public GameObject playerCamera;
    public TextMeshProUGUI situationText;
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
        mainCamera = GameObject.Find("MainCamera");
        situationText = GameObject.Find("SituationText").GetComponent<TextMeshProUGUI>();
    }

    private const float MaxStamina = 6f;

    [SerializeField]
    private Image staminaBar = default;

    private float currentStamina = MaxStamina;


    private void Update()
    {
        if (photonView.IsMine)
        {
            //var input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            //horizintal = Input.GetAxis("Horizontal");
            //vertintal = Input.GetAxis("Vertical");
            //if (input.sqrMagnitude > 0f)
            //{
            //    // 入力があったら、スタミナを減少させる
            //    currentStamina = Mathf.Max(0f, currentStamina - Time.deltaTime);
            //    transform.Translate(6f * Time.deltaTime * input.normalized);
            //    animator.speed = 1;
            //    if ((horizintal < 0 || horizintal>0))
            //    {
            //        animator.SetBool("Beside", true);
            //        animator.SetBool("Flont", false);
            //        animator.SetBool("Back", false);
            //        if (horizintal < 0)
            //        {
            //            transform.localScale = new Vector3(-1, 1, 1);
            //        }
            //        else
            //        {
            //            transform.localScale = new Vector3(1, 1, 1);
            //        }
            //    }
            //    //if(vertintal > 0)
            //    //{
            //    //    animator.SetBool("Beside", false);
            //    //    animator.SetBool("Flont", false);
            //    //    animator.SetBool("Back", true);
            //    //}
            //    //if (vertintal < 0)
            //    //{
            //    //    animator.SetBool("Beside", false);
            //    //    animator.SetBool("Flont", true);
            //    //    animator.SetBool("Back", false);
            //    //}

            //}
            //else
            //{
            //    // 入力がなかったら、スタミナを回復させる
            //    currentStamina = Mathf.Min(currentStamina + Time.deltaTime * 2, MaxStamina);
            //    animator.speed = 0;
            //}
            

            if(Input.GetAxis("Horizontal")!=0)
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


            //if(Input.GetAxis("Horizontal")!=0)
            //{
            //    if (rb.velocity.x < 3)
            //    {
            //        rb.AddForce(Vector3.right * Input.GetAxis("Horizontal") * 100);
            //    }
            //}
            //else
            //{
            //    animator.speed = 0;
            //}
           
        }

        // スタミナをゲージに反映する
        //staminaBar.fillAmount = currentStamina / MaxStamina;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="OutSide")
        {
            mainCamera.SetActive(true);
            playerCamera.SetActive(false);
            situationText.text = "YOUER LOST...";
            Destroy(this.gameObject);
        }
    }

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
