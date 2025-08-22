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

    public event Action<Scene> eventAfterActiveSceneChanged;
    private Action<enumMapType> delSetCurMapType;

    public sceneComponent(Action<enumMapType> parDelSetCurMapType) {
        // SceneInn is shell-scene for player to go into exile while unloading & reloading all other scenes
        if (!SceneManager.GetSceneByName("SceneInn").isLoaded) {
            SceneManager.LoadScene("SceneInn", LoadSceneMode.Additive);
        }

        delSetCurMapType = parDelSetCurMapType;
    }

    // sceneComponent's initiation requires each scene adds its delegate to eventAfterActiveSceneChanged, so it can't be done in creator
    public void init() {
        // scene load
            // SceneComponent should be created in gameManager, and gameManager should be created in SceneMenu, so SceneMenu's loading skipped
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

    #region transition
    public void transitionSceneMenu() {
        SceneManager.SetActiveScene(sceneMenu);

        eventAfterActiveSceneChanged(sceneMenu);
    }

    public void transitionSceneMap() {
        SceneManager.SetActiveScene(sceneMap);
                
        eventAfterActiveSceneChanged(sceneMap);
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
        SceneManager.SetActiveScene(sceneCombat);
        eventAfterActiveSceneChanged(sceneCombat);
        combatManager.CM.systemLevelEnter(parDataLevel, parArrUpgrade);
    }
    #endregion SceneLoad
}
