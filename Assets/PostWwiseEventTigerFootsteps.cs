using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventTigerFootsteps : MonoBehaviour
{
    public AK.Wwise.Event TigerFootsteps;
    // Start is called before the first frame update
    public void PlayTigerFootsteps()
    {
        TigerFootsteps.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
