/// <summary>
/// Lichtstraal van een lamp. Dodelijk voor de schaduw-vorm.
/// </summary>
public class LightZone : Zone
{
    public override bool IsDangerousFor(Shape shape)
    {
        return shape == Shape.Shadow;
    }
}
