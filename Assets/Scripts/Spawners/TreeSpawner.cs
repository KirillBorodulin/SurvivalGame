using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;
    private float spawnRadius = 2f;
    private float spawnHeight = 3f;
    [SerializeField]
    private int maxItems = 5;
    private int itemsSpawned = 0;

    public void Interact()
    {
        if (itemsSpawned < maxItems)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPos.y = transform.position.y + spawnHeight;
            GameObject item = Instantiate(itemPrefab, randomPos, Random.rotation);

            itemsSpawned++;

            if (itemsSpawned >= maxItems)
            {
                Destroy(gameObject, 0.5f);
            }
        }
    }
}