using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventTigerHurtVox : MonoBehaviour
{
    public AK.Wwise.Event TigerHurtVox;
    // Start is called before the first frame update
    public void PlayTigerHurtVox()
    {
        TigerHurtVox.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}