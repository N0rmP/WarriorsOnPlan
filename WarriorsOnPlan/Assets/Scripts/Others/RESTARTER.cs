using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// RESTARTER lives in SceneInn, its only mission is to unload & reload all other scenes (in other words, restarting the game totally)
public class RESTARTER : MonoBehaviour {
    // RestaRteR
    public static RESTARTER RRR { get; private set; } = null;

    public void Awake() {
        // singleton
        if (RRR == null) {
            RRR = this;
        } else {
            Destroy(this);
        }
        DontDestroyOnLoad(RRR);
    }

    // reloadAllScene reloads all scenes, just be aware of that SceneMenu becomes an active scene after method done
    public void reloadAllScene() {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("SceneInn"));
        // gameManager.GM is DontDestroyOnLoad and able to affect reloading negatively
        Destroy(gameManager.GM);
        StartCoroutine(unloadNReload());
    }

    public IEnumerator unloadNReload() {
        // find and unload all scenes except SceneInn,
        AsyncOperation tempAO = null;
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            if (SceneManager.GetSceneAt(i).name != "SceneInn") {
                tempAO = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
        }
        // wait until unloading done
        while (!tempAO.isDone) {
            yield return new WaitForSeconds(0.01f);
        }
        
        // load SceneMenu, gameManager is included in SceneMenu so it can do the rest job
        tempAO = SceneManager.LoadSceneAsync("SceneMenu", LoadSceneMode.Additive);
        // wait until loading done
        while (!tempAO.isDone) {
            yield return new WaitForSeconds(0.01f);
        }

        // ensure to return SceneMenu
        gameManager.GM.SceC.transitionSceneMenu();
    }
}