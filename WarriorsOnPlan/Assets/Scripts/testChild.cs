using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testChild : test {
    public override void testShout() {
        Debug.Log(gameObject + " : testChild shout");
    }
}
