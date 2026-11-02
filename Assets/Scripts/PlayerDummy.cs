using UnityEngine;

/// <summary>
/// Alleen voor testen in de Sandbox: een blok dat zich als speler gedraagt (tag "Player", IPlayer),
/// maar niet kan lopen. Sleep hem tijdens Play in de Scene-view. Kan weg zodra Osama's Player er is.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerDummy : MonoBehaviour, IPlayer
{
    [SerializeField] private Shape startShape;

    private Shape shape;
    private bool isDead;

    public Shape CurrentShape => shape;

    private void Awake()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        shape = startShape;
        UpdateColor();
    }

    public void Die()
    {
        // Zones roepen Die() elke physics-stap aan zolang de speler erin staat.
        if (isDead)
        {
            return;
        }

        isDead = true;
        Debug.Log($"{name} is dood ({shape}).", this);
        UpdateColor();
    }

    public void Respawn(Vector2 position)
    {
        transform.position = position;
        shape = startShape;
        isDead = false;
        UpdateColor();
    }

    private void UpdateColor()
    {
        Color color = shape == Shape.Light ? Palette.goud : Palette.doorn;
        GetComponent<SpriteRenderer>().color = isDead ? Color.gray : color;
    }

    [ContextMenu("Test: wissel vorm")]
    private void TestSwap()
    {
        shape = shape == Shape.Light ? Shape.Shadow : Shape.Light;
        UpdateColor();
    }

    [ContextMenu("Test: weer levend")]
    private void TestRevive()
    {
        Respawn(transform.position);
    }
}
