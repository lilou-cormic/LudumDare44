using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fall : MonoBehaviour
{
    private Rigidbody2D rb;

    public float FallSpeed = 20f;

    public bool IsFalling { get; private set; } = false;

    private RigidbodyType2D bodyType;

    private static int StaticLayer = 9;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void StartFalling()
    {
        bodyType = rb.bodyType;
        rb.bodyType = RigidbodyType2D.Kinematic;

        IsFalling = true;
        rb.velocity = Vector2.down * FallSpeed;
    }

    public void StopFalling()
    {
        IsFalling = false;
        rb.velocity = Vector2.zero;

        transform.position = new Vector3(transform.position.x, Mathf.FloorToInt(transform.position.y));

        rb.bodyType = bodyType;
    }

    public void SetStatic()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
            return;

        transform.position = new Vector3(transform.position.x, Mathf.RoundToInt(transform.position.y));

        StopFalling();

        rb.bodyType = RigidbodyType2D.Static;

        gameObject.layer = StaticLayer;
    }
}
