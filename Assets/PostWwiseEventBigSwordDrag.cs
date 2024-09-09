using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventBigSwordDrag : MonoBehaviour
{
    public AK.Wwise.Event BigSwordDrag;
    // Start is called before the first frame update
    public void PlayBigSwordDrag()
    {
      BigSwordDrag.Post(gameObject);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
