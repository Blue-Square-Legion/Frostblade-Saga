using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State 
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;

    public override void Enter()
    {
        IsComplete = false;
    }

    public override void Do()
    {
        print("Attack");
        IsComplete = true;
    }

    public override void Exit() { }

}
