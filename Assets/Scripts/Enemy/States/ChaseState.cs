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

    public override void Enter()
    {
        initScale = enemy.localScale;
    }

    public override void Do()
    {
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
}
