using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private GameObject followHeight;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, followHeight.transform.position.y + 5, transform.position.z);
    }
}
