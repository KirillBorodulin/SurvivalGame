using UnityEngine;
using UnityEngine.VFX;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;
    private float spawnRadius = 2f;
    private float spawnHeight = 3f;
    [SerializeField]
    private int maxItems = 5;
    private int itemsSpawned = 0;

    [SerializeField]
    private VisualEffect destroyEffect;

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
                if(destroyEffect != null)
                {
                    VisualEffect effect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
                    effect.Play();
                    Destroy(effect.gameObject, 2f);
                }
                Destroy(gameObject, 0.5f);
            }
        }
    }
}