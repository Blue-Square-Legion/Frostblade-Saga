using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventPlayerFootsteps : MonoBehaviour
{
    public AK.Wwise.Event PlayerFootstep;
    // Start is called before the first frame update
    public void PlayPlayerFootstep()
    {
        PlayerFootstep.Post(gameObject); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
