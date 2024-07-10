using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour {

    public static gameManager GM;
    public timerComponent TC { get; private set; }
    public jsonComponent JC { get; private set; }

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
    }
}
