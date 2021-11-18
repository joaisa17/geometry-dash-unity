using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPad : MonoBehaviour
{
    private bool debounce = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb != null && !debounce)
        {
            debounce = true;

            rb.gravityScale *= -1;

            float newVelocity = -rb.velocity.y;

            Player player = rb.gameObject.GetComponent<Player>();
            if (player != null) newVelocity =  (rb.gravityScale < 0 ? player.jumpingPower : -player.jumpingPower) / 2;

            rb.velocity = new Vector2(
                rb.velocity.x,
                newVelocity
            );
        }
    }
}
