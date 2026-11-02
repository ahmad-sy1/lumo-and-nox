using UnityEngine;

/// <summary>
/// Lamp die aan en uit kan. Aan: de lichtstraal (beam) is zichtbaar.
/// Op de beam zit een LightZone, die gevaarlijk is voor de schaduw-vorm.
/// Staat er een PushBox in de straal, dan stopt de straal bij de kist. Daaronder is schaduw.
/// Met onSprite/offSprite wisselt het plaatje van de lamp mee.
/// </summary>
public class Lamp : Activatable
{
    private const float BeamWidth = 0.8f;

    [SerializeField] private bool isOn = true;
    [SerializeField] private GameObject beam;
    [SerializeField] private float maxBeamLength = 6f;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private void FixedUpdate()
    {
        if (isOn && beam != null)
        {
            SetBeamLength(FindBeamLength());
        }
    }

    private float FindBeamLength()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.5f;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, new Vector2(BeamWidth, 0.05f), 0f, Vector2.down, maxBeamLength);

        float length = maxBeamLength;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.GetComponent<PushBox>() != null)
            {
                length = Mathf.Min(length, hit.distance);
            }
        }
        return length;
    }

    private void SetBeamLength(float length)
    {
        beam.transform.localScale = new Vector3(1f, length, 1f);
        beam.transform.localPosition = new Vector3(0f, -0.5f - length / 2f, 0f);
    }

    private void Start()
    {
        if (beam == null)
        {
            Debug.LogWarning($"Lamp '{name}' heeft geen beam ingesteld.", this);
            return;
        }

        if (isOn)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    public override void Activate()
    {
        isOn = true;
        SetBeam(true);
    }

    public override void Deactivate()
    {
        isOn = false;
        SetBeam(false);
    }

    private void SetBeam(bool active)
    {
        if (beam != null)
        {
            beam.SetActive(active);
        }

        Sprite sprite = active ? onSprite : offSprite;

        if (sprite != null && TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.sprite = sprite;
        }
    }
}
