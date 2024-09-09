using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventTigerAttackRoar : MonoBehaviour
{
    public AK.Wwise.Event TigerAttackRoar;
    // Start is called before the first frame update
    public void PlayTigerAttackRoar()
    {
        TigerAttackRoar.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
