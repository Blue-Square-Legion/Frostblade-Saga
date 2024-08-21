using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private Behaviour[] components;
    public float currentHealth { get; private set; }
    private bool dead;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        currentHealth = startingHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void TakeDamage(float _damage)
    {
        //reduce health
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
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
                    comp.enabled = false;
                }
                dead = true;
            }

        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = new Color(1, 0, 0, 0.5f);
        yield return new WaitForSeconds(1);
        spriteRenderer.color = Color.white;
    }
}
