using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventSmallSwordSwing : MonoBehaviour
{
    public AK.Wwise.Event SmallSwordSwing;
    // Start is called before the first frame update
    public void PlaySmallSwordSwing()
    {
        SmallSwordSwing.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}