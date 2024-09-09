using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventEnemyIdleCry : MonoBehaviour
{
    public AK.Wwise.Event EnemyIdleCry;
    // Start is called before the first frame update
    public void PlayEnemyIdleCry()
    {
        EnemyIdleCry.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
