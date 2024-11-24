using UnityEngine;

[ExecuteAlways]
public class BirdSpawnVolume : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Bounds bounds;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        bounds = boxCollider.bounds;
    }

    public Vector3 GetRandomPosition()
    {
        // Randomize position within the collider's world-space bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }

/*#if UNITY_EDITOR
    // Draw the spawn area in the Scene view
    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        if (!boxCollider) return;

        Gizmos.color = new Color(0, 1, 0, 0.3f); // Green with transparency
        Gizmos.DrawCube(boxCollider.bounds.center, boxCollider.bounds.size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
    }
#endif*/
}
