using UnityEngine;

/// <summary>
/// Wat zones en de LevelManager van een speler nodig hebben. Osama's Player implementeert dit
/// (zie docs/afspraken-player.md). In de Sandbox doet PlayerDummy dat zolang er nog geen Player is.
/// </summary>
public interface IPlayer
{
    Shape CurrentShape { get; }

    void Die();

    void Respawn(Vector2 position);
}
