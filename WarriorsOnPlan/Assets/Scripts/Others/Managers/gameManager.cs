using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour {

    public static gameManager GM;
    public excelComponent EC { get; private set; }
    public timerComponent TC { get; private set; }

    public void Awake() {
        if (GM == null) {
            GM = this;
        } else {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        //★ 세이브 파일 / 설정 모음집 참조하여 어떤 번역 쓸지 결정, 해상도 등 기본 초기화

        EC = new excelComponent();
        TC = gameObject.AddComponent<timerComponent>();
    }
}
