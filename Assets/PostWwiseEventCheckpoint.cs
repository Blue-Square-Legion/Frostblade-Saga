using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventCheckpoint : MonoBehaviour
{
    public AK.Wwise.Event Checkpoint;
    // Start is called before the first frame update
    public void PlayCheckpoint()
    {
        Checkpoint.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
