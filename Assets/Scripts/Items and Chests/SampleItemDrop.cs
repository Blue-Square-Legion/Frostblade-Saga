using UnityEngine;

public class ItemController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            PickUpItem(collision.gameObject);
        }
    }

    private void PickUpItem(GameObject player)
    {
        // we can add the part where effects happen in here.
        // Destroy the item after it's picked up
        Destroy(gameObject);
    }
}
