using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public GameObject player;
    public GameObject ground;
    public GameObject ceiling;

    public Vector2 positionOffset;

    public float moveTime = 0.5f;

    private Rigidbody2D rb;
    private Vector3 currentSpeed;

    private float originX;
    private float originY;

    private Camera cam;

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
        originY = transform.position.y;

        cam = Camera.main;

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

        float screenHeightInUnits = cam.ScreenToWorldPoint(new Vector3(
            0,
            Screen.height,
            0
        )).y - cam.ScreenToWorldPoint(new Vector3(
            0,
            0,
            0
        )).y;

        Vector3 targetPos = new Vector3(
            Mathf.Clamp(rb.position.x + positionOffset.x, originX, Mathf.Infinity),
            Mathf.Clamp(rb.position.y + positionOffset.y, originY, ceiling.transform.position.y - screenHeightInUnits),
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref currentSpeed,
            moveTime
        );

        ground.transform.position = new Vector3(
            transform.position.x,
            ground.transform.position.y,
            ground.transform.position.z
        );
    }
}
