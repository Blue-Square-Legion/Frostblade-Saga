using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float timeUntilDespawn;

    private GameManager gameManager;
    private float time = 0;
    private Animator animator;
    private bool hit;

    private void Start()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (hit) return;
        //Move the direction the projectile is facing
        transform.position += transform.right * Time.deltaTime * projectileSpeed;

        //Despawns projectile after certain amount of time
        if (time > timeUntilDespawn)
            Destroy(gameObject);
        time += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        if (collision.gameObject.CompareTag("Projectile"))
        {
            EnemyProjectile projectile = collision.gameObject.GetComponent<EnemyProjectile>();
            if (projectile != null)
            {
                projectile.FreezeProjectile();
                print("FREEZE PROJECTILE");
            }
        }

        if (collision.gameObject.TryGetComponent(out GenericEnemy enemy))
        {
            enemy.TakeDamage(1);
        }

        //Destroy self when it collides with anything
        animator.SetTrigger("collision");
        //Deactivate(), call in animator
    }

    public void Deactivate()
    {
        Destroy(gameObject);
    }
}
