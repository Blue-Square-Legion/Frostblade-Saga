using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : ContactDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifeTime;
    private BoxCollider2D boxCollider;

    private bool hit;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    public void ActivateProjectile()
    {
        lifeTime = 0;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifeTime += Time.deltaTime;
        if (lifeTime > resetTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision);
        boxCollider.enabled = false;
        gameObject.SetActive(false);
    }
}
