using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventBigSwordSwing : MonoBehaviour
{
    public AK.Wwise.Event BigSwordSwing;
    // Start is called before the first frame update
    public void PlayBigSwordSwing()
    {
        BigSwordSwing.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
