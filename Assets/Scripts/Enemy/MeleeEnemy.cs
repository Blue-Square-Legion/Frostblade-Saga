using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : GenericEnemy //Inherits from GenericEnemy
{
    [Header("Attack Parameters")]
    [SerializeField] private int damage;
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Health")]
    [SerializeField] private float meleeHealth;

    [Header("States")]
    public PatrolState patrolState;
    public ChaseState chaseState;
    public AttackState attackState;
    State state;

    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float colliderDistance;

    void Start()
    {
        health = Mathf.RoundToInt(meleeHealth);
        SelectState(patrolState);
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void SelectState(State _state)
    {
        state = _state;
        state.Enter();
    }

    private void Update()
    {
        if (PlayerInRange())
        {
            SelectState(attackState);
        }
        else if (PlayerInSight())
        {
            SelectState(chaseState);
        }   else
        {
            SelectState(patrolState);
        }
        if (state.IsComplete)
        {
            if (PlayerInSight())
            {
                SelectState(chaseState);
            } else
            {
                SelectState(patrolState);
            }
        }
    
        state.Do();
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * sightRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * sightRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private bool PlayerInRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * sightRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * sightRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
