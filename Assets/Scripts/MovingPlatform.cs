using UnityEngine;

/// <summary>
/// Platform dat heen en weer beweegt tussen pointA en pointB. Aan: het beweegt. Uit: het staat stil.
/// Een speler die erop staat, beweegt mee.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : Activatable
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool isMoving = true;

    private Rigidbody2D body;
    private Transform target;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        target = pointB;
    }

    private void FixedUpdate()
    {
        if (isMoving && pointA != null && pointB != null)
        {
            Move();
        }
    }

    public override void Activate()
    {
        isMoving = true;
    }

    public override void Deactivate()
    {
        isMoving = false;
    }

    private void Move()
    {
        Vector2 next = Vector2.MoveTowards(body.position, target.position, speed * Time.fixedDeltaTime);
        body.MovePosition(next);

        if (next == (Vector2)target.position)
        {
            target = target == pointA ? pointB : pointA;
        }
    }

    // De Player moet de tag "Player" hebben, anders beweegt hij niet mee (issue van Osama).
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // normal.y < -0.5: de speler staat bovenop het platform, niet ertegenaan.
        if (collision.gameObject.CompareTag("Player") && collision.GetContact(0).normal.y < -0.5f)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform.parent == transform)
        {
            collision.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}
