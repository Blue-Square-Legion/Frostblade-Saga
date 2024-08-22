using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //do damage to player upon collision
        if (collision.CompareTag("Player"))
            collision.GetComponent<Health>().TakeDamage(damage);
    }
}
