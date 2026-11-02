using UnityEngine;

/// <summary>
/// Basis voor alles wat aan en uit kan, zoals een deur, lamp of bewegend platform.
/// Een Lever of PressButton stuurt een Activatable aan als target.
/// </summary>
public abstract class Activatable : MonoBehaviour
{
    public abstract void Activate();

    public abstract void Deactivate();
}
