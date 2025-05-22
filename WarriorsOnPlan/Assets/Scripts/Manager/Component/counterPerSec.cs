using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counterPerSec : MonoBehaviour {
    // 500 is just arbitrary value to prevent malfunction when gameManager tries to use countPerSec without creating it
    public static int countPerSec { get; private set; } = 500;

    private float sec = 1f;
    private int count = 0;

    void Update() {
        sec -= Time.deltaTime;
        if (sec > 0f) {
            count++;
        } else {
            countPerSec = count;
            Destroy(this);
        }
    }
}
