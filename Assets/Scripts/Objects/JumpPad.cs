using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float boostHeight = 50f;

    private bool debounce = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb != null && !debounce)
        {
            debounce = true;

            rb.velocity = new Vector2(
                rb.velocity.x,
                boostHeight
            );
        }
    }
}
