using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Lopen, springen en de kracht van de huidige vorm voor één speler. Lumo en Nox zijn twee instanties
/// met andere instellingen: Lumo springt hoger (jumpForce), bij Nox staat canPushBoxes aan.
/// Licht-vorm: Gloed maakt LightPlatforms in de buurt zichtbaar. Schaduw-vorm: Schaduwstap laat de speler
/// door ShadowWalls lopen. De kracht is aan zolang de krachtknop ingedrukt is.
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
    [SerializeField] private Sprite lightSprite;
    [SerializeField] private Sprite shadowSprite;
    [SerializeField] private float glowRadius = 4f;
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
    private LightPlatform[] lightPlatforms;
    private bool[] glowing;
    private ShadowWall[] shadowWalls;
    private bool isUsingAbility;
    private Shape shape;
    private bool isDead;
    private bool isGrounded;
    private float moveInput;

    public bool CanPushBoxes => canPushBoxes;

    /// <summary>
    /// Wasd = Lumo, Pijltjes = Nox. De HUD gebruikt dit om de iconen goed te zetten.
    /// </summary>
    public Controls ControlScheme => controls;

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

        groundFilter = ContactFilter2D.noFilter;
        groundFilter.useTriggers = false;

        // Een hendel heeft een trigger als bereik, dus hier tellen triggers wel mee.
        interactFilter = ContactFilter2D.noFilter;

        ApplySprite();
    }

    private void Start()
    {
        lightPlatforms = FindObjectsByType<LightPlatform>();
        glowing = new bool[lightPlatforms.Length];
        shadowWalls = FindObjectsByType<ShadowWall>();
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
        bool ability = controls == Controls.Wasd ? keyboard.sKey.isPressed : keyboard.downArrowKey.isPressed;

        Move((right ? 1f : 0f) - (left ? 1f : 0f));

        if (jump)
        {
            Jump();
        }

        if (interact)
        {
            Interact();
        }

        if (ability && !isUsingAbility)
        {
            UseAbility();
        }
        else if (!ability && isUsingAbility)
        {
            StopAbility();
        }
    }

    private void FixedUpdate()
    {
        UpdateGrounded();
        body.linearVelocity = new Vector2(moveInput * moveSpeed, body.linearVelocity.y);

        if (isUsingAbility && shape == Shape.Light)
        {
            UpdateGlow();
        }
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
    /// Zet de kracht van de huidige vorm aan. Blijft aan tot StopAbility() of een andere vorm.
    /// </summary>
    public void UseAbility()
    {
        if (isDead || isUsingAbility)
        {
            return;
        }

        isUsingAbility = true;

        if (shape == Shape.Shadow)
        {
            foreach (ShadowWall wall in shadowWalls)
            {
                wall.SetPassable(box, true);
            }
        }
    }

    public void StopAbility()
    {
        if (!isUsingAbility)
        {
            return;
        }

        isUsingAbility = false;

        if (shape == Shape.Light)
        {
            for (int i = 0; i < lightPlatforms.Length; i++)
            {
                if (glowing[i])
                {
                    glowing[i] = false;
                    lightPlatforms[i].SetRevealed(false);
                }
            }
        }
        else
        {
            foreach (ShadowWall wall in shadowWalls)
            {
                wall.SetPassable(box, false);
            }

            PushOutOfWalls();
        }
    }

    /// <summary>
    /// Wissel van vorm (door de SwapManager). De kracht van de oude vorm gaat uit. Houden we de knop nog
    /// ingedrukt, dan start de kracht van de nieuwe vorm vanzelf.
    /// </summary>
    public void SetShape(Shape newShape)
    {
        StopAbility();
        shape = newShape;
        ApplySprite();
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

        StopAbility();
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

        isDead = false;
        spriteRenderer.color = Color.white;
        SetShape(startShape);
    }

    private void ApplySprite()
    {
        Sprite sprite = shape == Shape.Light ? lightSprite : shadowSprite;

        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    // Gloed: platforms binnen de straal zijn zichtbaar en betreedbaar, de rest niet.
    private void UpdateGlow()
    {
        for (int i = 0; i < lightPlatforms.Length; i++)
        {
            Vector2 offset = lightPlatforms[i].transform.position - transform.position;
            bool inRange = offset.sqrMagnitude <= glowRadius * glowRadius;

            if (inRange != glowing[i])
            {
                glowing[i] = inRange;
                lightPlatforms[i].SetRevealed(inRange);
            }
        }
    }

    // Staan we na Schaduwstap nog in een muur, dan duwen we de speler naar de dichtstbijzijnde vrije kant.
    private void PushOutOfWalls()
    {
        foreach (ShadowWall wall in shadowWalls)
        {
            Collider2D wallCollider = wall.GetComponent<Collider2D>();

            if (!box.Distance(wallCollider).isOverlapped)
            {
                continue;
            }

            Bounds playerBounds = box.bounds;
            Bounds wallBounds = wallCollider.bounds;
            float toLeft = playerBounds.max.x - wallBounds.min.x;
            float toRight = wallBounds.max.x - playerBounds.min.x;
            float push = toLeft < toRight ? -(toLeft + 0.01f) : toRight + 0.01f;

            body.position += new Vector2(push, 0f);
            transform.position = body.position;
        }
    }

    [ContextMenu("Test: wissel vorm")]
    private void TestSwapShape()
    {
        SetShape(shape == Shape.Light ? Shape.Shadow : Shape.Light);
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
