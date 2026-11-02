using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Lopen en springen voor één speler. Lumo en Nox zijn twee instanties met andere instellingen:
/// Lumo springt hoger (jumpForce), bij Nox staat canPushBoxes aan.
/// Zones en de LevelManager kennen de speler alleen via IPlayer (zie docs/afspraken-player.md).
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player : MonoBehaviour, IPlayer
{
    public enum Controls
    {
        Wasd,
        Arrows
    }

    private const float GroundCheckHeight = 0.05f;

    [SerializeField] private Controls controls;
    [SerializeField] private Shape startShape;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private bool canPushBoxes;

    private Rigidbody2D body;
    private BoxCollider2D box;
    private SpriteRenderer spriteRenderer;
    private ContactFilter2D groundFilter;
    private ContactFilter2D interactFilter;
    private readonly Collider2D[] groundHits = new Collider2D[4];
    private readonly Collider2D[] interactHits = new Collider2D[8];
    private Shape shape;
    private bool isDead;
    private bool isGrounded;
    private float moveInput;

    public bool CanPushBoxes => canPushBoxes;

    public Shape CurrentShape => shape;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        shape = startShape;

        body.freezeRotation = true;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Zonder wrijving blijf je niet aan een muur plakken als je ertegenaan springt.
        box.sharedMaterial = new PhysicsMaterial2D { friction = 0f, bounciness = 0f };

        groundFilter = new ContactFilter2D();
        groundFilter.NoFilter();
        groundFilter.useTriggers = false;

        // Een hendel heeft een trigger als bereik, dus hier tellen triggers wel mee.
        interactFilter = new ContactFilter2D();
        interactFilter.NoFilter();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null || isDead)
        {
            return;
        }

        bool left = controls == Controls.Wasd ? keyboard.aKey.isPressed : keyboard.leftArrowKey.isPressed;
        bool right = controls == Controls.Wasd ? keyboard.dKey.isPressed : keyboard.rightArrowKey.isPressed;
        bool jump = controls == Controls.Wasd ? keyboard.wKey.wasPressedThisFrame : keyboard.upArrowKey.wasPressedThisFrame;

        bool interact = controls == Controls.Wasd ? keyboard.eKey.wasPressedThisFrame : keyboard.rightShiftKey.wasPressedThisFrame;

        Move((right ? 1f : 0f) - (left ? 1f : 0f));

        if (jump)
        {
            Jump();
        }

        if (interact)
        {
            Interact();
        }
    }

    private void FixedUpdate()
    {
        UpdateGrounded();
        body.linearVelocity = new Vector2(moveInput * moveSpeed, body.linearVelocity.y);
    }

    /// <summary>
    /// direction: -1 naar links, 0 stilstaan, 1 naar rechts.
    /// </summary>
    public void Move(float direction)
    {
        if (isDead)
        {
            return;
        }

        moveInput = Mathf.Clamp(direction, -1f, 1f);

        if (moveInput != 0f && spriteRenderer != null)
        {
            spriteRenderer.flipX = moveInput < 0f;
        }
    }

    public void Jump()
    {
        UpdateGrounded();
        if (isDead || !isGrounded)
        {
            return;
        }

        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
    }

    /// <summary>
    /// Gebruikt de hendel waar we in staan (binnen het bereik van de trigger).
    /// </summary>
    public void Interact()
    {
        if (isDead)
        {
            return;
        }

        int count = box.Overlap(interactFilter, interactHits);

        for (int i = 0; i < count; i++)
        {
            if (interactHits[i].TryGetComponent(out Lever lever))
            {
                lever.Use();
                return;
            }
        }
    }

    /// <summary>
    /// Mag vaker achter elkaar aangeroepen worden, zones doen dat elke physics-stap.
    /// </summary>
    public void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        moveInput = 0f;
        spriteRenderer.color = Color.gray;

        LevelManager levelManager = FindAnyObjectByType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.OnPlayerDied();
        }
    }

    public void Respawn(Vector2 position)
    {
        body.position = position;
        transform.position = position;
        body.linearVelocity = Vector2.zero;

        shape = startShape;
        isDead = false;
        spriteRenderer.color = Color.white;
    }

    // Een smalle strook net onder de voeten: raakt die iets dat geen trigger is, dan staan we op de grond.
    private void UpdateGrounded()
    {
        Bounds bounds = box.bounds;
        Vector2 center = new Vector2(bounds.center.x, bounds.min.y - GroundCheckHeight / 2f);
        Vector2 size = new Vector2(bounds.size.x * 0.9f, GroundCheckHeight);

        int count = Physics2D.OverlapBox(center, size, 0f, groundFilter, groundHits);

        isGrounded = false;
        for (int i = 0; i < count; i++)
        {
            if (groundHits[i] != box)
            {
                isGrounded = true;
                return;
            }
        }
    }
}
