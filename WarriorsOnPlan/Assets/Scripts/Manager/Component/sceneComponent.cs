using Cases;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneComponent {
    private Scene sceneMenu;
    private Scene sceneMap;
    private Scene sceneCombat;

    private GameObject cameraCombat;

    public event Action<Scene> eventAfterActiveSceneChanged;

    public sceneComponent() {
        cameraCombat = GameObject.Find("CAMERA_Combat");
        cameraCombat.SetActive(false);
    }

    // sceneComponent's initiation requires each scene adds its delegate to eventAfterActiveSceneChanged, so it can't be done in creator
    public void init() {
        // scene load, SceneMenu is the first scene and already loaded, so you need to load other scens here
        SceneManager.LoadScene("SceneMap", LoadSceneMode.Additive);
        SceneManager.LoadScene("SceneCombat", LoadSceneMode.Additive);

        sceneMenu = SceneManager.GetSceneByName("SceneMenu");
        sceneMap = SceneManager.GetSceneByName("SceneMap");
        sceneCombat = SceneManager.GetSceneByName("SceneCombat");

        SceneManager.activeSceneChanged += (x, y) => {
            gameManager.GM.TC.clearDelegate();
            gameManager.GM.UC.clearAll();
        };
    }

    public IEnumerable<Scene> enumerateLoadedScene() {
        yield return sceneMenu;
        yield return sceneMap;
        yield return sceneCombat;
    }

    #region transition
    public void transitionSceneMenu() {
        cameraCombat.SetActive(false);
        SceneManager.SetActiveScene(sceneMenu);
        eventAfterActiveSceneChanged(sceneMenu);
    }

    public void transitionSceneMap() {
        cameraCombat.SetActive(false);
        SceneManager.SetActiveScene(sceneMap);

        eventAfterActiveSceneChanged(sceneMap);
        mapManager.MM.prepareMap(gameManager.GM.curMapType);
    }

    public void transitionSceneCombat(int parLevelCode, upgradeAbst[] parArrUpgrade) {
        if (parLevelCode / 10000 == 9) {
            Debug.Log("gameManager.loadSceneCombat tries to load text level with LevelCode " + parLevelCode);
        }

        transitionSceneCombat(
            gameManager.GM.DHouC.getDataLevel(parLevelCode),
            parArrUpgrade
        );
    }

    public void transitionSceneCombat(dataLevel parDataLevel, upgradeAbst[] parArrUpgrade) {
        cameraCombat.SetActive(true);
        SceneManager.SetActiveScene(sceneCombat);
        eventAfterActiveSceneChanged(sceneCombat);
        combatManager.CM.systemLevelEnter(parDataLevel, parArrUpgrade);
    }
    #endregion SceneLoad
}
