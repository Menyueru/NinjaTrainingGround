using UnityEngine;

public class FinishLine : MonoBehaviour {

    [HideInInspector]
    public bool passed = false;
    public AudioSource audioSource;

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            if (!passed)
            {
                audioSource.Play();
                passed = true;
            }
        }
    }
}
