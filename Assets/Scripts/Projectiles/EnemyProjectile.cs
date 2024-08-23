using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : ContactDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifeTime;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb; 

    private bool hit;
    private bool isFrozen; 

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>(); 
    }

    public void ActivateProjectile()
    {
        
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
            gameObject.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFrozen) return;
        hit = true;
        base.OnTriggerEnter2D(collision);
        boxCollider.enabled = false;
        gameObject.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("Enemy");

    }


    public void FreezeProjectile()
    {
        isFrozen = true;
        rb.velocity = Vector2.zero; // freeze projectile movement
        //rb.isKinematic = true;
        boxCollider.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Ground");
    }
}


