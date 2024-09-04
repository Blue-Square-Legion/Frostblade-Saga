using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float chargeCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private float chargeTimer = 0;

    [SerializeField] private GameObject[] fireballs;
    private Vector3[] firepoints = new Vector3[5];

    private bool attacking = false;
    private bool charging = true;

    private void Start()
    {
        //populate firepoint array
        for (int i = 0; i < fireballs.Length; i++)
        {
            firepoints[i] = fireballs[i].transform.position;
        }
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (attacking) return;

        if (charging)
        {
            Charge();
        }
        else if (PlayerInClawRange())
        {
            if (cooldownTimer >= attackCooldown)
            {
                StartClawAttack();
            }
        }
        else if (chargeTimer >= chargeCooldown)
        {
            StartChargeAttack();
        }
        else
        {
            if (!hasActiveFireball() && cooldownTimer >= attackCooldown)
            {
                StartWaveAttack();
            }
        }
    }

    void StartClawAttack()
    {
        attacking = true;
        //set animation trigger
        
        DoClawDamage();
        //^move to animation event once we have animation
    }

    void DoClawDamage()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center +
            transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider.gameObject.TryGetComponent(out Health player))
        {
            player.TakeDamage(1);
            //player hit
        }

        //reset cooldown
        cooldownTimer = 0;
        attacking = false;
    }

    void StartChargeAttack()
    {
        charging = true;
        //trigger animation
    }

    void Charge()
    {
        transform.position += speed * Time.deltaTime * transform.right * Mathf.Sign(transform.localScale.x);
    }

    void StartWaveAttack()
    {
        //set animation trigger
        //spawn fireballs
        attacking = true;
        StartCoroutine(SpawnFireballs());

        //shoot fireballs
        //reset cooldown
        cooldownTimer = 0;
        attacking = false;
    }

    private bool PlayerInClawRange()
    {

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + 
            transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    IEnumerator SpawnFireballs()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            GameObject temp = fireballs[i];
            temp.transform.position = firepoints[i];
            temp.GetComponent<EnemyProjectile>().ActivateProjectile();
            temp.GetComponent<EnemyProjectile>().SetWaiting(true);
            yield return new WaitForSeconds(.5f);
        }

        for (int i = 0; i < fireballs.Length; i++)
        {
            fireballs[i].GetComponent<EnemyProjectile>().SetWaiting(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //stop charging
        if (charging)
        {
            charging = false;
        }
    }

    private bool hasActiveFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (fireballs[i].activeInHierarchy)
                return true;
        }
        return false;
    }

}
