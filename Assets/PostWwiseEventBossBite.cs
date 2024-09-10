using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventBossBite : MonoBehaviour
{
    public AK.Wwise.Event BossBite;
    // Start is called before the first frame update
    public void PlayBossBite()
    {
        BossBite.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
