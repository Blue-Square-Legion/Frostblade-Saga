using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform player;
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Stopping points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    public override void Enter()
    {
        initScale = enemy.localScale;
    }

    public override void Do()
    {
        //if ((enemy.position.x <= leftEdge.position.x) && (player.position.x < leftEdge.position.x)
        //    || (enemy.position.x >= rightEdge.position.x) && (player.position.x > rightEdge.position.x)) return;
        if (enemy.position.x >= player.position.x)
            MoveInDirection(-1);
        else
        {
            MoveInDirection(1);
        }
    }

    public override void Exit() { }

    private void MoveInDirection(int _direction)
    {
        //flip enemy sprite to face the correct direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        //move enemy
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }

    public bool PlayerInReach()
    {
        return !((enemy.position.x <= leftEdge.position.x) && (player.position.x < leftEdge.position.x)
             || (enemy.position.x >= rightEdge.position.x) && (player.position.x > rightEdge.position.x));
    }
}
