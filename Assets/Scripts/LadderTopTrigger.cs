using UnityEngine;
using System.Collections;

public class LadderTopTrigger : MonoBehaviour
{
    [HideInInspector]
    public bool onLadderTop = false;

    public void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            onLadderTop = true;
        }
    }

    public void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            onLadderTop = false;
        }
    }
}
