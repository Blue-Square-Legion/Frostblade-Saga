using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventChestOpen : MonoBehaviour
{
    public AK.Wwise.Event ChestOpen;
    // Start is called before the first frame update
    public void PlayChestOpen()
    {
        ChestOpen.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}