using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject[] itemPrefabs; // Array to hold different item prefabs
    [SerializeField] private int numberOfItemsToSpawn = 3; // Number of items to spawn
    [SerializeField] private Vector2 spawnOffset = new Vector2(0, 0.5f); // Offset for item spawning
    [SerializeField] private float itemSpreadForce = 2f; // Force to spread items out

    private bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpened && collision.CompareTag("Player"))
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpened = true;

        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            SpawnItem();
        }

        // leaving space here to add an animation or sound effect to indicate the chest is opened
        
    }

    private void SpawnItem()
    {
        // Choose a random item prefab from the array
        GameObject randomItem = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        // Determine the spawn position near the chest
        Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;

        // Instantiate the item at the spawn position
        GameObject spawnedItem = Instantiate(randomItem, spawnPosition, Quaternion.identity);

        // Apply a random spread force to the item
        Rigidbody2D itemRb = spawnedItem.GetComponent<Rigidbody2D>();
        if (itemRb != null)
        {
            Vector2 randomForce = new Vector2(Random.Range(-1f, 1f), 1f) * itemSpreadForce;
            itemRb.AddForce(randomForce, ForceMode2D.Impulse);
        }
    }
}
