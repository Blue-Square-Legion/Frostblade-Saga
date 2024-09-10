using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventBossProjectile : MonoBehaviour
{
    public AK.Wwise.Event BossProjectile;
    // Start is called before the first frame update
    public void PlayBossProjectile()
    {
        BossProjectile.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
