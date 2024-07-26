using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiPersonalCanvas : MonoBehaviour
{
    void LateUpdate() {
        transform.LookAt(transform.position + gameManager.GM.camera.transform.forward);
    }
}

