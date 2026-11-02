using UnityEngine;

/// <summary>
/// Drukknop: het target staat aan zolang er een speler (tag "Player") of een kist op staat,
/// en gaat uit als er niets meer op staat.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class PressButton : MonoBehaviour
{
    [SerializeField] private Activatable target;
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;

    private bool isPressed;
    private int objectsOnButton;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (CanPress(other))
        {
            objectsOnButton++;
            UpdateState();
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (CanPress(other))
        {
            objectsOnButton = Mathf.Max(0, objectsOnButton - 1);
            UpdateState();
        }
    }

    private bool CanPress(Collider2D other)
    {
        return other.CompareTag("Player") || other.GetComponent<PushBox>() != null;
    }

    private void UpdateState()
    {
        bool pressed = objectsOnButton > 0;
        if (pressed == isPressed)
        {
            return;
        }

        isPressed = pressed;

        if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Sprite sprite = isPressed ? downSprite : upSprite;
            if (sprite != null)
            {
                spriteRenderer.sprite = sprite;
            }
        }

        if (target == null)
        {
            Debug.LogWarning($"PressButton '{name}' heeft geen target.", this);
        }
        else if (isPressed)
        {
            target.Activate();
        }
        else
        {
            target.Deactivate();
        }
    }

    [ContextMenu("Test: speler erop")]
    private void TestStepOn()
    {
        objectsOnButton++;
        UpdateState();
    }

    [ContextMenu("Test: speler eraf")]
    private void TestStepOff()
    {
        objectsOnButton = Mathf.Max(0, objectsOnButton - 1);
        UpdateState();
    }
}
