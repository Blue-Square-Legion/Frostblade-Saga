using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State 
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float attackCooldown;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private Animator animator;

    public override void Enter()
    {
        IsComplete = false;
    }

    public override void Do()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer > attackCooldown)
        {
            animator.SetTrigger("attack");
            cooldownTimer = 0;
            print("Attack");
        }
        
        IsComplete = true;
        
    }

    public override void Exit() { }

}
