using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventBossFootsteps : MonoBehaviour
{
    public AK.Wwise.Event BossFootstep;
    // Start is called before the first frame update
    public void PlayBossFootstep()
    {
        BossFootstep.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}