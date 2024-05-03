using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempMover : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(-1f, 0f, 0f);
    }
}
