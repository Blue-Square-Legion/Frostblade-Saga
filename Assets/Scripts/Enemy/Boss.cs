using UnityEngine;

public class Boss : GenericEnemy
{
    [SerializeField] float speed = 10;
    [SerializeField] int bossHealth = 50;
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float chargeCooldown;
    [SerializeField] private int clawDamage = 1;
    [SerializeField] private float clawRange;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;

    [SerializeField] private GameObject[] fireballs;

    [SerializeField] Transform player;
    [SerializeField] Transform firepoint;
    [SerializeField] GameObject fireball;

    [SerializeField] Transform leftEdge;
    [SerializeField] Transform rightEdge;

    private Vector3 initScale;

    private bool charging = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        initScale = transform.localScale;
        health = Mathf.RoundToInt(bossHealth);
        startingHealth = Mathf.RoundToInt(bossHealth);
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (transform.position.x < leftEdge.position.x)
        {
            charging = false;
            cooldownTimer = 0;
            transform.position = new Vector3(transform.position.x+1,transform.position.y,transform.position.z);
            animator.SetTrigger("stopRun");
        } else if (transform.position.x > rightEdge.position.x)
        {
            charging = false;
            cooldownTimer = 0;
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            animator.SetTrigger("stopRun");
        }

        if (charging)
        {
            transform.position += speed * Time.deltaTime * transform.right * Mathf.Sign(transform.localScale.x);
        }

        if (cooldownTimer < attackCooldown) return;
            charging = false;
        //look in player direction
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(initScale.x) * 1, initScale.y, initScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(initScale.x) * -1, initScale.y, initScale.z);

        if (UnityEngine.Random.Range(0f, 1f) > 0.7f &&
            Mathf.Abs(player.position.x - transform.position.x) > 8 &&
            !(transform.position.x < leftEdge.position.x || transform.position.x > rightEdge.position.x))
        {
            StartChargeAttack();
        }
        else if (PlayerInClawRange())
        {
            StartClawAttack();
        }
        else
        {
            StartWaveAttack();
        }
    }

    void StartClawAttack()
    {
        //set animation trigger
        animator.SetTrigger("claw");
        //DoClawDamage(); called in animator
    }

    void DoClawDamage()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center +
            transform.right * clawRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * clawRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider.gameObject.TryGetComponent(out Health player))
        {
            player.TakeDamage(clawDamage);
            //player hit
        }

        //reset cooldown
        cooldownTimer = 0;
        animator.ResetTrigger("claw");
    }

    void StartChargeAttack()
    {
        //trigger animation
        animator.SetTrigger("run");
        //Charge(); called in animator
    }

    void Charge()
    {
        charging = true;
    }

    void StartWaveAttack()
    {
        //set animation trigger
        animator.SetTrigger("ranged");
        //SpawnFireballs(); called in animator
    }

    private bool PlayerInClawRange()
    {

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + 
            transform.right * clawRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * clawRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * clawRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * clawRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    void SpawnFireballs()
    {
        for (int i = 0; i < fireballs.Length; i++)
            {
                GameObject temp = fireballs[i];
                temp.transform.position = firepoint.position;
                temp.GetComponent<EnemyProjectile>().ActivateProjectile();
            }
        cooldownTimer = 0;
        animator.ResetTrigger("ranged");
    }
    public void DeactivateFireballs()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            fireballs[i].SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile")) return;
        //stop charging
        if (charging)
        {
            charging = false;
            cooldownTimer = 0;
            transform.position -= transform.right * Mathf.Sign(transform.localScale.x);
            animator.SetTrigger("stopRun");
        }
        
    }
    public void BossRespawn()
    {
        health = startingHealth;
    }
}
