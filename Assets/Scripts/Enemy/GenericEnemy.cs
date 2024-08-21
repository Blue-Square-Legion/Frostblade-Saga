using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Parent class
public abstract class GenericEnemy : MonoBehaviour
{
    // Initializes Varibales
    [SerializeField] protected int health;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float attackRange;
}
