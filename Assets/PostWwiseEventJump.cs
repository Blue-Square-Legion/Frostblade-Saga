using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventJump : MonoBehaviour
{
    public AK.Wwise.Event PlayerJump;
    // Start is called before the first frame update
    public void PlayPlayerJump()
    {
        PlayerJump.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
