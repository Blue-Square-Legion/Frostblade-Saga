using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventEnemyAttackCry : MonoBehaviour
{
    public AK.Wwise.Event EnemyAttackCry;
    // Start is called before the first frame update
    public void PlayEnemyAttackCry()
    {
        EnemyAttackCry.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
