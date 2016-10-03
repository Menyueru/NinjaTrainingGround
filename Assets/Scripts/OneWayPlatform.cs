using UnityEngine;
using System.Collections;

public class OneWayPlatform : MonoBehaviour
{
    [HideInInspector]
    public bool oneway = false;

    public void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            oneway = true;
        }
    }

    public void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            oneway = false;
        }
    }
}
