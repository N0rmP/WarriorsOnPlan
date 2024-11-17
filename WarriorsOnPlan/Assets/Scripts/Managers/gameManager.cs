using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {

    public static gameManager GM;
    public jsonComponent JC { get; private set; }
    public timerComponent TC { get; private set; }
    public uiFxComponent UC { get; private set; }
    public dragComponent DC { get; private set; }

    public Camera cameraMain { get; private set; }
    public optionAIO option { get; private set; }

    public void Awake() {
        if (GM == null) {
            GM = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //★ 세이브 파일 / 설정 모음집 참조하여 어떤 번역 쓸지 결정, 해상도 등 기본 초기화

        JC = new jsonComponent();
        TC = gameObject.AddComponent<timerComponent>();
        UC = gameObject.AddComponent<uiFxComponent>();
        DC = gameObject.AddComponent<dragComponent>();

        SceneManager.sceneLoaded += findCamera;
        findCamera(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        option = new optionAIO();
    }

    private void findCamera(Scene parScene, LoadSceneMode parLSM) {
        this.cameraMain = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    public bool checkFileExist(string parPath) {
        //return File.Exists(@"./")
        return true;
    }
}
