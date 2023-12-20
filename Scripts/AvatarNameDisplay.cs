using Photon.Pun;
using TMPro;
public class AvatarNameDisplay : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        var  nameLabel = GetComponent<TextMeshPro>();
        nameLabel.text = $"{photonView.Owner.NickName}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
