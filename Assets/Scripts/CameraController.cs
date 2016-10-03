using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform player;
    [HideInInspector]
    public Vector2 velocity;
    float smoothTime = 0.4f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            float x = Mathf.SmoothDamp(transform.position.x,
                                      player.position.x, ref velocity.x, smoothTime);
            float y = Mathf.SmoothDamp(transform.position.y,
                                       player.position.y, ref velocity.y, smoothTime);
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
