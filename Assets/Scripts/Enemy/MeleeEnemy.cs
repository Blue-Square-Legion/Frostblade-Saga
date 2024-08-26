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

    public PatrolState patrolState;
    public ChaseState chaseState;
    State state;

    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float colliderDistance;

    void Start()
    {
        SelectState(patrolState);
        boxCollider= GetComponent<BoxCollider2D>();
    }

    void SelectState(State _state)
    {
        state = _state;
        state.Enter();
    }

    private void Update()
    {
        if (PlayerInSight())
        {
            print("chasing");
            SelectState(chaseState);
        }   else
        {
            print("patrolling");
            SelectState(patrolState);
        }
        if (state.IsComplete)
        {
            //if (PlayerInRange())
            //{
            //    SelectState(chaseState);
            //}
            if (PlayerInSight())
            {
                print("chasing");
                SelectState(chaseState);
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
    }
}
