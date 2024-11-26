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
    public Canvas canvasMain { get; private set; }
    public optionAIO option { get; private set; }

    public void Awake() {
        if (GM == null) {
            GM = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //�� ���̺� ���� / ���� ������ �����Ͽ� � ���� ���� ����, �ػ� �� �⺻ �ʱ�ȭ

        JC = new jsonComponent();
        TC = gameObject.AddComponent<timerComponent>();
        UC = gameObject.AddComponent<uiFxComponent>();
        DC = gameObject.AddComponent<dragComponent>();

        SceneManager.sceneLoaded += findCC;
        findCC(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        option = new optionAIO();
    }

    private void findCC(Scene parScene, LoadSceneMode parLSM) {
        this.cameraMain = GameObject.Find("MainCamera").GetComponent<Camera>();
        this.canvasMain = GameObject.Find("CANVAS").GetComponent<Canvas>();
    }

    public bool checkFileExist(string parPath) {
        //return File.Exists(@"./")
        return true;
    }
}
