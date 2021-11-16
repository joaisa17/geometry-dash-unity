using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player;
    public GameObject ground;

    public Vector2 positionOffset;

    public float moveTime = 0.5f;

    private Rigidbody2D rb;
    private float currentSpeed = 0;

    private float originX;

    private bool active = false;

    public void ResetState()
    {
        active = false;
    }

    public void Init()
    {
        active = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        originX = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!active) return;

        float targetPos = Mathf.Clamp(rb.position.x + positionOffset.x, originX, Mathf.Infinity);

        float newPos = Mathf.SmoothDamp(
            transform.position.x,
            targetPos,
            ref currentSpeed,
            moveTime
        );

        transform.position = new Vector3(
            newPos,
            transform.position.y + positionOffset.y,
            transform.position.z
        );

        ground.transform.position = new Vector3(
            transform.position.x,
            ground.transform.position.y,
            ground.transform.position.z
        );
    }
}
