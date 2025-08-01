using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonEnterMapMenu : MonoBehaviour {
    public void click() {
        gameManager.GM.SceC.transitionSceneMap();
    }
}
