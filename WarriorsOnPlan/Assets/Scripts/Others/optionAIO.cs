using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumTranslation { 
    english,
    korean
}

public class optionAIO
{
    public static int screenWidth = 1920;
    public static int screenHeight = 1080;

    public static enumTranslation curTranslation = enumTranslation.english;

    // ★ option 내역을 저장해두는 json 파일을 하나 만들 것, 게임 실행 시 가장 먼저 그것을 가져와 화면과 언어 등을 설정할 것
}
