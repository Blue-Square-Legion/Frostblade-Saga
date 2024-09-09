using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventSwordGrow : MonoBehaviour
{
    public AK.Wwise.Event SwordGrow;
    // Start is called before the first frame update
    public void PlaySwordGrow()
    {
        SwordGrow.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}