using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventEnemyHitCry : MonoBehaviour
{
    public AK.Wwise.Event EnemyHitCry;
    // Start is called before the first frame update
    public void PlayEnemyHitCry()
    {
        EnemyHitCry.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
