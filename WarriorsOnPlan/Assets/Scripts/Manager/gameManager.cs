using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class gameManager : MonoBehaviour {

    public static gameManager GM;
    public fileComponent FC { get; private set; }
    public timerComponent TC { get; private set; }    
    public dragComponent DC { get; private set; }
    public inputComponent IC { get; private set; }
    public makerComponent MC { get; private set; }
    public popupComponent PC { get; private set; }
    public optionAIO option { get; private set; }
    public uiFxComponent UC { get; private set; }
    public dataHouseComponent DHouC { get; private set; }
    public saveComponent SaveC { get; private set; }
    public sceneComponent SceC { get; private set; }
    public linerComponent LC { get; private set; }

    // ★ 랜덤 함수 필요해지면 (그래픽 말고 실제 처리 과정에서) xoshiro 만들었던 거 가져와서 randomComponenet 만드셈

    public Canvas canvasMain { get; private set; }    

    [NonSerialized]
    public enumMapType curMapType;

    public void Awake() {
        if (GM == null) {
            GM = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //★ 세이브 파일 / 설정 모음집 참조하여 어떤 번역 쓸지 결정, 해상도 등 기본 초기화

        SceC = new sceneComponent();
        // findCanvasMain should be the first delegate in eventAfterActiveSceneChanged because most UI methods should reference it
        SceC.eventAfterActiveSceneChanged += findCanvasMain;
        // SceC.init includes each scene's main canvas activating, without it several works regarding canvas may fail
        SceC.init();
        findCanvasMain(SceneManager.GetActiveScene());

        TC = gameObject.AddComponent<timerComponent>();
        UC = gameObject.AddComponent<uiFxComponent>();
        DC = gameObject.AddComponent<dragComponent>();
        IC = gameObject.AddComponent<inputComponent>();
        PC = new popupComponent();
        option = new optionAIO();
        FC = new fileComponent();
        MC = new makerComponent();
        DHouC = new dataHouseComponent();
        SaveC = new saveComponent();
        LC = new linerComponent();

        curMapType = enumMapType.Normal;    //★ 테스트를 위해 임의로 normal로 설정함, 추후 none으로 변경하고 난이도 선택 시 수정케할 것
    }

    private void findCanvasMain(Scene parScene) {
        this.canvasMain = gameObject.FindThoroughly("CANVAS_" + SceneManager.GetActiveScene().name).GetComponent<Canvas>();
    }
}

/*
public readonly struct bookBasicWords {
    public readonly string strMelee;
    public readonly string strNumber;
    public readonly string strReady;
    public readonly string strAlertNoAttackTarget;
    public readonly string strAlertNoSkillTarget;
    public readonly string strQuestionResetInitial;

    public bookBasicWords(enumTranslation parEnumTranslation) {
        dataBookBasicWords tempDataBasicWord = gameManager.GM.FC.importResourcesJson<dataBookBasicWords>("BasicWord");
        strMelee = tempDataBasicWord.strMelee;
        strNumber = "(" + tempDataBasicWord.strNumber + ")";
        strReady = tempDataBasicWord.strReady;
        strAlertNoAttackTarget = tempDataBasicWord.strAlertNoAttackTarget;
        strAlertNoSkillTarget = tempDataBasicWord.strAlertNoSkillTarget;
        strQuestionResetInitial = tempDataBasicWord.strQuestionResetInitial;
    }
}
*/
