using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CANVAS : MonoBehaviour {
    public void Start() {
        gameManager.GM.SceC.eventAfterActiveSceneChanged += swapCANVAS;
        swapCANVAS(SceneManager.GetActiveScene());
    }

    private void swapCANVAS(Scene parScene) {
        if (parScene.name == gameObject.name.Substring(7)) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }
}
