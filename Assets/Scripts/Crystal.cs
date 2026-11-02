using UnityEngine;

/// <summary>
/// Kristal dat een speler kan oppakken. Telt +1 bij de LevelManager en verdwijnt.
/// Een opgepakt kristal blijft opgepakt, ook als de spelers daarna doodgaan.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Crystal : MonoBehaviour
{
    private bool isCollected;
    private LevelManager levelManager;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (isCollected)
        {
            return;
        }

        isCollected = true;
        if (levelManager != null)
        {
            levelManager.AddCrystal();
        }
        gameObject.SetActive(false);
    }

    [ContextMenu("Test: oppakken")]
    private void TestCollect()
    {
        Collect();
    }
}
