using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventPlayerAttackVox : MonoBehaviour
{
    public AK.Wwise.Event PlayerAttackVox;
    // Start is called before the first frame update
    public void PlayPlayerAttackVox()
    {
        PlayerAttackVox.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}