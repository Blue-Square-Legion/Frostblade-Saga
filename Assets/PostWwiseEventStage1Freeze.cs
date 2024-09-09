using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventStage1Freeze : MonoBehaviour
{
    public AK.Wwise.Event Stage1Freeze;
    // Start is called before the first frame update
    public void PlayStage1Freeze()
    {
        Stage1Freeze.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}