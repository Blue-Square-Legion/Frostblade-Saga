using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventIceAura : MonoBehaviour
{
    public AK.Wwise.Event IceAura;
    // Start is called before the first frame update
    public void PlayIceAura()
    {
        IceAura.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}