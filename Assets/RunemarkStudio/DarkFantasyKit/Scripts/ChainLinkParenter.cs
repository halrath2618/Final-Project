using UnityEngine;

public class ChainLinkParenter : MonoBehaviour
{
    void Awake()
    {
        GetComponent<CharacterJoint>().connectedBody = transform.parent.GetComponent<Rigidbody>();
    }

}