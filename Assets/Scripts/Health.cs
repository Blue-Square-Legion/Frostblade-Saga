using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private Behaviour[] components;
    public float CurrentHealth { get; private set; } //private set because only TakeDamage() should be able to modify this. Can be changed later
    private bool dead;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        CurrentHealth = startingHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    public void TakeDamage(float _damage)
    {
        //reduce health
        CurrentHealth = CurrentHealth - _damage;

        animator.SetTrigger("Hurt");

        if (CurrentHealth > 0)
        {
            //hurt
        }
        else
        {
            //dead
            if (!dead)
            {
                rb.velocity = Vector3.zero;
                foreach (Behaviour comp in components)
                {
                    //disable rather than destroy in case we want to respawn
                    comp.enabled = false;
                }
                dead = true;
                UIManager.Instance.GameOver();
            }

        }
    }

    public void Heal(float healAmount)
    {
        // Increase current health but don't exceed the starting health
        CurrentHealth = Mathf.Min(CurrentHealth + healAmount, startingHealth);
    }
}
