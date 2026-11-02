/// <summary>
/// Donkere poel. Dodelijk voor de licht-vorm.
/// </summary>
public class DarkZone : Zone
{
    public override bool IsDangerousFor(Shape shape)
    {
        return shape == Shape.Light;
    }
}
