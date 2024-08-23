using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : GenericEnemy //Inherits from GenericEnemy
{
    [SerializeField] private float meleeHealth;


    public PatrolState patrolState;
    State state;

    void Start()
    {
        health = Mathf.RoundToInt(meleeHealth);
        SelectState();
    }

    void SelectState()
    {
        state = patrolState;
        state.Enter();
    }

    private void Update()
    {
        if (state.IsComplete)
        {
            SelectState();
        }
        state.Do();
    }

    private bool PlayerInSight()
    {
        return false;
    }

    private bool PlayerInRange()
    {
        return false;
    }
}
