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

    private void Start()
    {
        CurrentHealth = startingHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void TakeDamage(float _damage)
    {
        //reduce health
        CurrentHealth = CurrentHealth - _damage;

        if (CurrentHealth > 0)
        {
            //hurt
            StartCoroutine(FlashRed());
        }
        else
        {
            //dead
            if (!dead)
            {
                foreach (Behaviour comp in components)
                {
                    //disable rather than destroy in case we want to respawn
                    comp.enabled = false;
                }
                dead = true;
            }

        }
    }

<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
    public void Heal(float healAmount)
    {
        // Increase current health but don't exceed the starting health
        CurrentHealth = Mathf.Min(CurrentHealth + healAmount, startingHealth);
    }

    private IEnumerator FlashRed()
    {
        //once the player sprite is implemented, Color.white sets it to the original palette rather that pure white.
        spriteRenderer.color = new Color(1, 0, 0, 0.5f);
        yield return new WaitForSeconds(1);
        spriteRenderer.color = Color.white;
    }
}
