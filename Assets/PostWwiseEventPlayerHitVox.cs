using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventPlayerHitVox : MonoBehaviour
{
    public AK.Wwise.Event PlayerHitVox;
    // Start is called before the first frame update
    public void PlayPlayerHitVox()
    {
        PlayerHitVox.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}