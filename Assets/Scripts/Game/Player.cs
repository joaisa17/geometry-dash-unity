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
    public float rotationResetTime = 0.5f;

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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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

        else StartCoroutine(Die());
    }

    // Runs once per frame
    void Update()
    {
        if (!active) return;

        if (!dead)
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
            rb.angularVelocity *= 0.5f;

            if (!isTouchingObject) rb.SetRotation(rb.rotation - rotationSpeed * Time.fixedDeltaTime);


            // Loop through jump keybinds
            for (int i = 0; i < jumpKeys.Length; i++)
            {
                if (Input.GetKey(jumpKeys[i])) Jump();
            }

            // Jump with mouse
            if (enableJumpWithMouse && Input.GetMouseButton(0)) Jump();
        }
    }

    IEnumerator Die()
    {
        dead = true;

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        deathEffect.Play();
        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(1);
        dieEvent.Invoke();
    }
}
