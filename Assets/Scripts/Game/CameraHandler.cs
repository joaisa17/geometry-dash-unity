using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
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

        Camera cam = Camera.main;

        float camHeight = cam.orthographicSize * 2;
        float camWidth = camHeight * Screen.width / Screen.height;

        Vector3 currentScale = ground.transform.localScale;

        ground.transform.localScale = new Vector3(
            camWidth + 5,
            currentScale.y,
            currentScale.z
        );

        SpriteRenderer groundRenderer = ground.GetComponent<SpriteRenderer>();

        Vector2 screenBounds = cam.ScreenToWorldPoint(new Vector3(
            0,
            0,
            cam.transform.position.z
        ));

        ground.transform.position = new Vector3(
            ground.transform.position.x,
            screenBounds.y + groundRenderer.bounds.extents.y,
            ground.transform.position.z
        );
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
