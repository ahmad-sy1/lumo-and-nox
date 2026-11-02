using System.Collections;
using UnityEngine;

/// <summary>
/// Kist die alleen beweegt als Nox Push() aanroept. Verder staat hij vast in de breedte, dus Lumo
/// kan hem niet wegduwen. Een kist onder een lamp blokkeert de lichtstraal (zie Lamp).
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PushBox : MonoBehaviour
{
    private const float PushTimeout = 0.1f;
    private const RigidbodyConstraints2D Locked =
        RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

    [SerializeField] private float pushSpeed = 2f;

    private Rigidbody2D body;
    private float lastPushTime = float.MinValue;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.constraints = Locked;
    }

    public void Push(Vector2 direction)
    {
        if (Mathf.Approximately(direction.x, 0f))
        {
            return;
        }

        lastPushTime = Time.time;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        body.linearVelocity = new Vector2(Mathf.Sign(direction.x) * pushSpeed, body.linearVelocity.y);
    }

    private void FixedUpdate()
    {
        // Duwt Nox niet meer, dan staat de kist weer vast.
        if (Time.time - lastPushTime > PushTimeout && body.constraints != Locked)
        {
            body.linearVelocity = new Vector2(0f, body.linearVelocity.y);
            body.constraints = Locked;
        }
    }

    [ContextMenu("Test: Nox duwt naar links")]
    private void TestPushLeft()
    {
        StartCoroutine(PushFor(Vector2.left, 0.75f));
    }

    [ContextMenu("Test: Nox duwt naar rechts")]
    private void TestPushRight()
    {
        StartCoroutine(PushFor(Vector2.right, 0.75f));
    }

    private IEnumerator PushFor(Vector2 direction, float seconds)
    {
        float end = Time.time + seconds;
        while (Time.time < end)
        {
            Push(direction);
            yield return new WaitForFixedUpdate();
        }
    }
}
