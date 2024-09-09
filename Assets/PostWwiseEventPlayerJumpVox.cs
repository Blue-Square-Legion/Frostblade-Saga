using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventPlayerJumpVox : MonoBehaviour
{
    public AK.Wwise.Event PlayerJumpVox;
    // Start is called before the first frame update
    public void PlayPlayerJumpVox()
    {
        PlayerJumpVox.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}