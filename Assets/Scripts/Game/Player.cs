using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public Collider2D finishTrigger;
    public ParticleSystem deathEffect;

    public KeyCode[] jumpKeys = {KeyCode.Space};
    public bool enableJumpWithMouse = true;

    // Physics
    public float jumpingPower = 5f;
    public float movementSpeed = 5f;

    // Rotation
    public float rotationSpeed = 5f;

    // Events
    public UnityEvent winEvent;
    public UnityEvent dieEvent;

    private Rigidbody2D rb;
    private Vector2 defaultPos;

    private bool active = false;
    private bool canJump = true;
    private bool isTouchingObject = true;

    private bool dead = false;

    // Runs when script has initialized
    void Awake()
    {
        Camera cam = Camera.main;

        Vector2 screenBounds = cam.ScreenToWorldPoint(new Vector3(
            Screen.width,
            Screen.height,
            cam.transform.position.z
        ));

        rb = GetComponent<Rigidbody2D>();
        rb.position = new Vector2(
            -screenBounds.x - GetComponent<SpriteRenderer>().bounds.extents.x,
            rb.position.y
        );

        defaultPos = rb.position;
    }

    // Runs when level has started
    public void Init()
    {
        active = true;

        rb.WakeUp();
        rb.velocity.Set(movementSpeed, rb.velocity.y);
    }

    // Runs when level is resetting
    public void ResetState()
    {
        active = false;
        dead = false;

        rb.position.Set(defaultPos.x, defaultPos.y);
        rb.velocity.Set(0, 0);
        rb.Sleep();
    }

    void Jump()
    {
        if (!active || !canJump || dead) return;
        canJump = false;

        rb.angularVelocity = 0;
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
    }

    private void OnCollisionEnter2D()
    {
        canJump = true;
        isTouchingObject = true;
    }

    private void OnCollisionExit2D()
    {
        canJump = false;
        isTouchingObject = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == finishTrigger) winEvent.Invoke();

        else Die();
    }

    // Runs once per frame
    void Update()
    {
        if (!active) return;

        if (!dead)
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);

            if (!isTouchingObject)
            {
                rb.SetRotation(rb.rotation - rotationSpeed * Time.fixedDeltaTime);
            }

            else
            {
                rb.angularVelocity /= 1.01f;
            }


            // Loop through jump keybinds
            for (int i = 0; i < jumpKeys.Length; i++)
            {
                if (Input.GetKey(jumpKeys[i])) Jump();
            }

            // Jump with mouse
            if (enableJumpWithMouse && Input.GetMouseButton(0)) Jump();
        }
    }

    void Die()
    {
        dead = true;

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        deathEffect.Play();
        GetComponent<SpriteRenderer>().enabled = false;

        dieEvent.Invoke();
    }
}
