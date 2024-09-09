using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWwiseEventBigSwordDrag : MonoBehaviour
{
    public AK.Wwise.Event BigSwordDrag;
    // Start is called before the first frame update
    public void StopBigSwordDrag()
    {
        BigSwordDrag.Stop(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
