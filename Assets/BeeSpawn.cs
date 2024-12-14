using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSpawn : MonoBehaviour
{
    public GameObject beePrefab;
    public int beeCount = 10;
    public Transform flowersParent;
    private BoxCollider boxCollider;
    private Bounds bounds;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        for (int i = 0; i < beeCount; i++)
        {
            GameObject bee = Instantiate(beePrefab, GetRandomPosition(), Quaternion.identity);

            bee.GetComponent<BeeController>().flowersParent = flowersParent;
        }
    }

    public Vector3 GetRandomPosition()
    {
        bounds = boxCollider.bounds;
        // Randomize position within the collider's world-space bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }
}
