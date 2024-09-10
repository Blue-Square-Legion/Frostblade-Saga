using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Parent class
public abstract class GenericEnemy : MonoBehaviour
{
    // Initializes Variables
    [SerializeField] protected int health;
    protected int startingHealth;
    protected Animator animator;
   

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
            AkSoundEngine.PostEvent("Play_EnemyHitCry", gameObject);
        } else
        {
            animator.SetTrigger("hurt");
        }
    }
    protected virtual void Die()
    {
        // extra logics after enemy dies (e.g., play animation, drop items)
        gameObject.SetActive(false);
        if (gameObject.CompareTag("Boss"))
            UIManager.Instance.VictoryScreen();
    }

    public virtual void Respawn()
    {
        health = startingHealth;
        gameObject.SetActive(true);
    }
}





