using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {

    public static gameManager GM;
    public timerComponent TC { get; private set; }
    public jsonComponent JC { get; private set; }

    public GameObject camera { get; private set; }

    public void Awake() {
        if (GM == null) {
            GM = this;
        } else {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        //�� ���̺� ���� / ���� ������ �����Ͽ� � ���� ���� ����, �ػ� �� �⺻ �ʱ�ȭ

        TC = gameObject.AddComponent<timerComponent>();
        JC = new jsonComponent();

        SceneManager.sceneLoaded += (x, y) => { this.camera = GameObject.Find("MainCamera"); };
    }
}
