using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventPlayerLanding : MonoBehaviour
{
    public AK.Wwise.Event PlayerLanding;
    // Start is called before the first frame update
    public void PlayPlayerLanding()
    {
        PlayerLanding.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
