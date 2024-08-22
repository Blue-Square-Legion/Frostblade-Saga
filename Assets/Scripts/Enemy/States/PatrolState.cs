using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;


    public override void Enter() 
    {
        initScale = enemy.localScale;
    }

    public override void Do() 
    {
        if (movingLeft)
        {
            //Go left until reaching left edge
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
            {
                DirectionChange();
            }
        }
        else
        {
            //Go right until reaching right edge
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
            {
                DirectionChange();
            }
        }
    }

    public override void Exit() { }


    // Idle for a while then turn around and move in opposite direction
    private void DirectionChange()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        //flip enemy sprite to face the correct direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        //move enemy
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
}
