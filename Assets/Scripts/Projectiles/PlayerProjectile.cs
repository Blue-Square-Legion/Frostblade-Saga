using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float timeUntilDespawn;

    private GameManager gameManager;
    private float time = 0;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    void FixedUpdate()
    {
        //Move the direction the projectile is facing
        transform.position += transform.right * Time.deltaTime * projectileSpeed;

        //Despawns projectile after certain amount of time
        if (time > timeUntilDespawn)
            Destroy(gameObject);
        time += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If projectile hit an enemy
        if (collision.gameObject.TryGetComponent(out GenericEnemy enemy))
        {
            print("ENEMY HIT");
            enemy.TakeDamage(1);
        }

        //Destroy self when it collides with anything
        Destroy(gameObject);
    }
}
