using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {

    public static gameManager GM;
    public jsonComponent JC { get; private set; }
    public timerComponent TC { get; private set; }
    public uiFxComponent UC { get; private set; }

    public GameObject camera { get; private set; }

    public void Awake() {
        if (GM == null) {
            GM = this;
        } else {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        //�� ���̺� ���� / ���� ������ �����Ͽ� � ���� ���� ����, �ػ� �� �⺻ �ʱ�ȭ

        JC = new jsonComponent();
        TC = gameObject.AddComponent<timerComponent>();
        UC = gameObject.AddComponent<uiFxComponent>();

        SceneManager.sceneLoaded += (x, y) => { this.camera = GameObject.Find("MainCamera"); };
    }
}
