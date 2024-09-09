using UnityEngine;

public class EnemyProjectile : ContactDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;

    private float lifeTime;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private bool hit;
    private bool isFrozen;
    private Animator animator;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void ActivateProjectile()
    {
        //reset to default fireball
        if (isFrozen)
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            spriteRenderer.color = Color.white;
            rb.bodyType = RigidbodyType2D.Dynamic;
            boxCollider.isTrigger = true;
        }

        lifeTime = 0;
        gameObject.SetActive(true);
        if(boxCollider != null )
            boxCollider.enabled = true;
        hit = false;
        isFrozen = false;  
        
    }

    private void Update()
    {
        if (hit) return;

        if (!isFrozen)
        {
            float movementSpeed = speed * Time.deltaTime;
            transform.Translate(movementSpeed, 0, 0);
        }

        lifeTime += Time.deltaTime;
        if (lifeTime > resetTime)
        {
            //reset to default fireball
            gameObject.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            spriteRenderer.color = Color.white;
            rb.bodyType = RigidbodyType2D.Dynamic;
            boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && (!isFrozen && !collision.GetComponent<EnemyProjectile>().isFrozen)) return;
        if (collision.gameObject.CompareTag("PlayerProjectile"))
            FreezeProjectile();
        else
        {
            hit = true;
            base.OnTriggerEnter2D(collision);
            boxCollider.enabled = false;
            animator.SetTrigger("collide");
            spriteRenderer.color = Color.white;
            //Deactivate(), call in animator
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
    }


    public void FreezeProjectile()
    {
        isFrozen = true;
        boxCollider.isTrigger = false;
        rb.velocity = Vector2.zero; // freeze projectile movement
        spriteRenderer.color = Color.cyan;
        //rb.isKinematic = true;
        rb.bodyType = RigidbodyType2D.Static;
        boxCollider.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Ground");
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}


