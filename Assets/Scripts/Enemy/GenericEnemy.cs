using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Parent class
public abstract class GenericEnemy : MonoBehaviour
{
    // Initializes Variables
    [SerializeField] protected int health;
    //[SerializeField] protected float walkSpeed;
    //[SerializeField] protected int attackDamage;
    //[SerializeField] protected float attackRange;

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        // extra logics after enemy dies (e.g., play animation, drop items)
        gameObject.SetActive(false);
    }
}





