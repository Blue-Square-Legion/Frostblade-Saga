using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventNPCVox : MonoBehaviour
{
    public AK.Wwise.Event NPCVox;
    // Start is called before the first frame update
    public void PlayNPCVox()
    {
        NPCVox.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}