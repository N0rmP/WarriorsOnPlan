using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class inputComponent : MonoBehaviour{
    Dictionary<string, inputContainer> dictSceneInputContainer;
    private inputContainer curInputContainer = null;
    // if inputContainer should change only during few seconds, the original container rests in lameInputContainer
    private inputContainer lameInputContainer;

    public void Awake() {
        dictSceneInputContainer = new Dictionary<string, inputContainer>();
        Scene tempScene;
        for (int i=0; i<SceneManager.sceneCount; i++) {
            tempScene = SceneManager.GetSceneAt(i);
            dictSceneInputContainer.Add(tempScene.name, new inputContainer());
        }

        gameManager.GM.SceC.eventAfterActiveSceneChanged += (x) => {
            curInputContainer = getInputContainer(x.name);
        };
    }

    public void Update() {
        curInputContainer?.updateInput();
    }

    private inputContainer getInputContainer(string parSceneName) {
        if (!dictSceneInputContainer.ContainsKey(parSceneName)) {
            Debug.Log("inputComponent.addDel treid to work with invalid parSceneName " + parSceneName);
            return null;
        }
        
        return dictSceneInputContainer[parSceneName];
    }

    #region relay

    public void addKeyActionPair(string parSceneName, KeyCode parKeyCode, Action parDel) {
        getInputContainer(parSceneName)?.addKeyActionPair(parKeyCode, parDel);
    }

    public void removeKey(string parSceneName, KeyCode parKeyCode) {
        getInputContainer(parSceneName)?.removeKey(parKeyCode);
    }

    public void removeKeyAction(string parSceneName, KeyCode parKeyCode, Action parDel) {
        getInputContainer(parSceneName)?.removeKeyAction(parKeyCode, parDel);
    }

    public void addAnyKeyAction(string parSceneName, Action parDel, bool parIsReplace = false) {
        getInputContainer(parSceneName)?.addAnyKeyAction(parDel, parIsReplace);
    }

    public void addScrollUp(string parSceneName, Action parDel, bool parIsReplace = false) {
        getInputContainer(parSceneName)?.addScrollUp(parDel);
    }

    public void addScrollDown(string parSceneName, Action parDel, bool parIsReplace = false) {
        getInputContainer(parSceneName)?.addScrollDown(parDel);
    }
    #endregion relay

    #region temporary
    public void inaugurateTemporayInputContinaer(inputContainer parTIC) {
        lameInputContainer = curInputContainer;
        curInputContainer = parTIC;
    }

    public void dismissTemporayInputContinaer() {
        curInputContainer = lameInputContainer;
        lameInputContainer = null;
    }
    #endregion temporary
}
