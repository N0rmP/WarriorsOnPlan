using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class canvasSceneMenu : MonoBehaviour {
    public void Start() {
        transform.FindThoroughly("textButtonEnterMap").GetComponent<TextMeshProUGUI>().text = gameManager.GM.DHouC.bookUI.strStartGame;
        transform.FindThoroughly("textButtonOption").GetComponent<TextMeshProUGUI>().text = gameManager.GM.DHouC.bookUI.strOption;
        transform.FindThoroughly("textButtonQuit").GetComponent<TextMeshProUGUI>().text = gameManager.GM.DHouC.bookUI.strQuit;
        transform.FindThoroughly("textButtonGraphTheHilt").GetComponent<TextMeshProUGUI>().text = gameManager.GM.DHouC.bookUI.strGrabTheHilt;
        transform.FindThoroughly("textButtonBrandishTheSword").GetComponent<TextMeshProUGUI>().text = gameManager.GM.DHouC.bookUI.strBrandishTheSword;
    }
}