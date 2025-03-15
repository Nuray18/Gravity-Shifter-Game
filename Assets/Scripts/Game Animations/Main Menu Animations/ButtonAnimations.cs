using UnityEngine;

public class ButtonAnimations : MonoBehaviour
{
    private Rigidbody2D rb;
    private static Vector2 gravityDirection;  // Gravity yönü

    private static bool gravitySet = false;  // Gravity sadece bir kere ayarlansın
    private string WALL_TAG = "Wall";

    private float startingSpeed = 200f;
    private int gravityScale = 10;
    private float gravity = 9.81f;
    private float mass = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityScale;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        rb.drag = 0.05f;
        rb.angularDrag = 0.05f;

        rb.mass = mass;

        if (!gravitySet)
        {
            SetRandomGravityDirection();
            gravitySet = true;
        }

        // Global gravity yönünü değiştiriyoruz
        Physics2D.gravity = gravityDirection * gravity;

        // Başlangıçta hız veriyoruz
        rb.velocity = gravityDirection * startingSpeed;
    }

    void SetRandomGravityDirection()
    {
        int randomDir = Random.Range(0, 3);

        switch (randomDir)
        {
            case 0: gravityDirection = Vector2.down; break;
            case 1: gravityDirection = Vector2.left; break;
            case 2: gravityDirection = Vector2.right; break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(WALL_TAG))
        {
            rb.velocity = Vector2.zero;
        }
    }
}