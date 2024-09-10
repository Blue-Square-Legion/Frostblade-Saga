using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventBossChargeRoar : MonoBehaviour
{
    public AK.Wwise.Event BossChargeRoar;
    // Start is called before the first frame update
    public void PlayBossChargeRoar()
    {
        BossChargeRoar.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
