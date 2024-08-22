using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : GenericEnemy //Inherits from GenericEnemy
{
    public PatrolState patrolState;
    State state;

    void Start()
    {
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
}
