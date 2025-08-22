using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonOptionApply : MonoBehaviour {
    public void click() {
        gameManager.GM.option.confirmCO();
    }
}
