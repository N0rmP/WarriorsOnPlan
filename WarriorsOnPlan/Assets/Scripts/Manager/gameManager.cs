using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class gameManager : MonoBehaviour {

    public static gameManager GM;
    public jsonComponent JC { get; private set; }
    public timerComponent TC { get; private set; }    
    public dragComponent DC { get; private set; }
    public basicInputComponent BIC { get; private set; }
    public makerComponent MC { get; private set; }
    public popupComponent PC { get; private set; }
    public optionAIO option { get; private set; }
    public uiFxComponent UC { get; private set; }

    // ★ 랜덤 함수 필요해지면 (그래픽 말고 실제 처리 과정에서) xoshiro 만들었던 거 가져와서 randomComponenet 만드셈

    public Canvas canvasMain { get; private set; }

    public event UnityAction<Scene, LoadSceneMode> doWhenSceneLoaded2;

    public bookBasicWords book { get; private set; }

    public void Awake() {
        if (GM == null) {
            GM = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //★ 세이브 파일 / 설정 모음집 참조하여 어떤 번역 쓸지 결정, 해상도 등 기본 초기화

        //doWhenSceneLoaded += findCanvasMain;
        SceneManager.sceneLoaded += findCanvasMain;
        
        TC = gameObject.AddComponent<timerComponent>();
        UC = gameObject.AddComponent<uiFxComponent>();
        DC = gameObject.AddComponent<dragComponent>();
        BIC = gameObject.AddComponent<basicInputComponent>();
        PC = new popupComponent();
        option = new optionAIO();
        JC = new jsonComponent();
        MC = new makerComponent();

        // doWhenSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        //SceneManager.sceneLoaded += doWhenSceneLoaded;

        setBook(option.curTranslation);

        // counterPerSec estimates how many it iterates during 1 second, ★ 나중에 봐서 필요성 낮으면 그냥 Time.deltaTime 쓰게 의존한 거 다 바꾸고 삭제
        gameObject.AddComponent<counterPerSec>();
    }

    private void findCanvasMain(Scene parScene, LoadSceneMode parLSM) {
        this.canvasMain = GameObject.Find("CANVAS").GetComponent<Canvas>();
    }

    // ★
    public bool checkFileExist(string parPath) {
        // return File.Exists(@"./");
        return true;
    }

    public void setBook(enumTranslation parEnumTranslation) {
        book = new bookBasicWords(parEnumTranslation);
    }
}

public readonly struct bookBasicWords {
    public readonly string strMelee;
    public readonly string strNumber;
    public readonly string strReady;
    public readonly string strAlertNoAttackTarget;
    public readonly string strAlertNoSkillTarget;
    public readonly string strQuestionResetInitial;

    public bookBasicWords(enumTranslation parEnumTranslation) {
        dataBookBasicWords tempDataBasicWord = gameManager.GM.JC.getJson<dataBookBasicWords>("BasicWord");
        strMelee = tempDataBasicWord.strMelee;
        strNumber = "(" + tempDataBasicWord.strNumber + ")";
        strReady = tempDataBasicWord.strReady;
        strAlertNoAttackTarget = tempDataBasicWord.strAlertNoAttackTarget;
        strAlertNoSkillTarget = tempDataBasicWord.strAlertNoSkillTarget;
        strQuestionResetInitial = tempDataBasicWord.strQuestionResetInitial;
}
}
