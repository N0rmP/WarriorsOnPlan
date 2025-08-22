using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonOptionReset : MonoBehaviour {
    public void click() {
        gameManager.GM.option.resetOption();
    }
}
