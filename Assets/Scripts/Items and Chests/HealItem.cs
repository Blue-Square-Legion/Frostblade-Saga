using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float healAmount = 1f; // The amount of health restored by this item

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        Health health = collision.GetComponent<Health>();

        if (health != null)
        {
            // Restore health
            health.Heal(healAmount);

            Destroy(gameObject);
        }
    }
}
