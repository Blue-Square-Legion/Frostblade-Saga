using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventBossRoar : MonoBehaviour
{
    public AK.Wwise.Event BossRoar;
    // Start is called before the first frame update
    public void PlayBossRoar()
    {
        BossRoar.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
