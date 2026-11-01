using UnityEngine;

/// <summary>
/// Lamp die aan en uit kan. Aan: de lichtstraal (beam) is zichtbaar.
/// Op de beam komt later een LightZone, die gevaarlijk is voor de schaduw-vorm.
/// </summary>
public class Lamp : Activatable
{
    [SerializeField] private bool isOn = true;
    [SerializeField] private GameObject beam;

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
    }
}
