using UnityEngine;

/// <summary>
/// Hendel die een target aan- of uitzet. De Player roept Use() aan als hij binnen bereik
/// (de trigger-collider) op E of Rechter Shift drukt.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Lever : MonoBehaviour
{
    [SerializeField] private Activatable target;
    [SerializeField] private bool isOn;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void Start()
    {
        UpdateSprite();
    }

    public void Use()
    {
        isOn = !isOn;
        UpdateSprite();

        if (target == null)
        {
            Debug.LogWarning($"Lever '{name}' heeft geen target.", this);
            return;
        }

        if (isOn)
        {
            target.Activate();
        }
        else
        {
            target.Deactivate();
        }
    }

    private void UpdateSprite()
    {
        Sprite sprite = isOn ? onSprite : offSprite;

        if (sprite != null && TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.sprite = sprite;
        }
    }

    [ContextMenu("Test: Use")]
    private void TestUse()
    {
        Use();
    }
}
