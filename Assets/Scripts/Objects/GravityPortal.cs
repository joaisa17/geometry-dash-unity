using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPortal : MonoBehaviour
{
    public bool inverse = false;

    private bool debounce = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb != null && !debounce)
        {
            debounce = true;

            rb.gravityScale = inverse ?
            rb.gravityScale > 0 ? -rb.gravityScale : rb.gravityScale :
            rb.gravityScale < 0 ? -rb.gravityScale : rb.gravityScale;
        }
    }
}
