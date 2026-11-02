/// <summary>
/// Stekels of een afgrond. Dodelijk voor beide vormen.
/// </summary>
public class HazardZone : Zone
{
    public override bool IsDangerousFor(Shape shape)
    {
        return true;
    }
}
