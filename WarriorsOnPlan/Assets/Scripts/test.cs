using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI tempTMP = null;
        int tempindex = tempTMP.text.IndexOf("text");
        TMP_TextInfo tempTextInfo = tempTMP.textInfo;
        TMP_CharacterInfo tempCharInfo = tempTextInfo.characterInfo[tempindex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
