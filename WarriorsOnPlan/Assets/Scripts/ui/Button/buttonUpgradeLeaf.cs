using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Cases;

/*
    BulCode is not coherent with codableObject's code
    BulCode's each digit represents below information
        first digit (highest)       : upgrade tree number (ascending from 1, not zero)
        second digit                : layer in the tree (ascending from 1, not zero)
        last two digits (lowest)    : numer in the layer (ascending from 1, not zero)
*/
public class buttonUpgradeLeaf : MonoBehaviour, IReadyToBeSearched {
    // upgraded buttonUpgradeLeaf has yellow frame, matUIColorIgnorer helps the frame ignore the sprite's original color and become yellow
    private static Material matUIColorIgnorer = null;

    public bool isVisited { get; set; }
    public int thisLeafCode { get; private set; }
    public bool isUpgraded { get; private set; }
    private upgradeAbst thisUpgrade = null;
    private List<buttonUpgradeLeaf> listPrev;
    private List<buttonUpgradeLeaf> listNext;

    public void Awake() {
        if (matUIColorIgnorer == null) {
            matUIColorIgnorer = Resources.Load<Material>("Material/materialUIColorIgnorer");
        }

        listPrev = new List<buttonUpgradeLeaf>();
        listNext = new List<buttonUpgradeLeaf>();
    }

    public void init(int parButtonUpgradeLeafCode, upgradeAbst parUpgrade) {
        GetComponent<buttonCustom>().interactable = false;
        clearPrev();
        clearNext();

        thisLeafCode = parButtonUpgradeLeafCode;
        isUpgraded = false;
        thisUpgrade = parUpgrade;
        GetComponent<showerCase>().setCase(thisUpgrade);
        transform.GetChild(0).GetComponent<Image>().sprite = thisUpgrade.caseImage;
        transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = thisUpgrade.starRequired.ToString();
        mapManager.MM.UC.undoUpgradeTemporary(thisLeafCode, true);
    }

    private void setFrameImage(Color parColor, bool parIsIgnoreSpriteColor) {
        Image tempImage = transform.GetChild(1).GetComponent<Image>();
        
        if (parIsIgnoreSpriteColor) {
            tempImage.material = matUIColorIgnorer;
            tempImage.material.SetColor("_Color", parColor);
        } else {
            tempImage.material = null;
            tempImage.color = parColor;
        }
    }

    private void startUpdateChain() {
        Queue<buttonUpgradeLeaf> tempQueBUL = new Queue<buttonUpgradeLeaf>();
        tempQueBUL.Enqueue(this);

        mapManager.MM.MUC.CU.setAllUnvisited();
        buttonUpgradeLeaf tempCurBUL;
        while (tempQueBUL.Count > 0) {
            tempQueBUL.Dequeue().updateChain(tempQueBUL);
        }
    }

    #region including_do
    public void click() {
        if (isUpgraded || !GetComponent<buttonCustom>().interactable) {
            return;
        }

        if (mapManager.MM.UC.starTemporary < thisUpgrade.starRequired) {
            gameManager.GM.PC.popupBasicAlert(GetComponent<RectTransform>().convertAcrossRect(gameManager.GM.canvasMain.GetComponent<RectTransform>(), new Vector3(0f, 0f, 0f)), "test : not enough stars");
            return;
        }

        mapManager.MM.UC.doUpgradeTemporay(thisLeafCode, thisUpgrade);
        isUpgraded = true;
        startUpdateChain();
        GetComponent<showerCase>().deshow();
    }

    public void rightClick() {
        if (!isUpgraded) {
            return;
        }

        mapManager.MM.UC.undoUpgradeTemporary(thisLeafCode);
        isUpgraded = false;
        startUpdateChain();
        GetComponent<showerCase>().deshow();        
    }

    // undoUpgrade all next upgrades, please call it with argument-true to frontline the first buttonUpgradeLeaf
    private void updateChain(Queue<buttonUpgradeLeaf> parQueue) {
        // this buttonUpgradeLeaf keeps its isUpgraded true if at least one prev-buttonUpgradeLeaf is still done
        bool tempIsParentDone = listPrev.Count == 0;
        foreach (buttonUpgradeLeaf bul in listPrev) {
            if (bul.isUpgraded) {                
                tempIsParentDone = true;
                break;
            }
        }

        if (tempIsParentDone) {
            if (isUpgraded) {
                paintDone();
            } else {
                paintFrontline();
            }
        } else {
            mapManager.MM.UC.undoUpgradeTemporary(thisLeafCode);
            paintUndone();
        }

        foreach (buttonUpgradeLeaf bul in listNext) {
            if (!bul.isVisited) {
                bul.isVisited = true;
                parQueue.Enqueue(bul);
            }
        }
    }

    public void systemDoUpgradeTemporary() {
        mapManager.MM.UC.doUpgradeTemporay(thisLeafCode, thisUpgrade, true);
        paintDone();
        foreach (buttonUpgradeLeaf bul in listNext) {
            bul.paintFrontline();
        }
    }
    #endregion including_do

    #region paint
    // frontline means upgrade-available, prev-upgrade is done and this upgrade is opened
    public void paintFrontline() { 
        isUpgraded = false;
        setFrameImage(Color.white, false);
        GetComponent<buttonCustom>().interactable = true;
    }

    private void paintDone() {
        isUpgraded = true;
        // make imageFrame Ignores original color, and set its color
        setFrameImage(Color.yellow, true);
        GetComponent<buttonCustom>().interactable = true;        
    }

    // undoUpgrade is for public calling and contains frontlining this buttonUpgradeLeaf, please use undoUpgradeChian for what you think
    private void paintUndone() {
        isUpgraded = false;
        setFrameImage(Color.white, false);
        GetComponent<buttonCustom>().interactable = false;
    }    
    #endregion paint

    #region next_prev
    public void addNext(buttonUpgradeLeaf parBUL) {
        if (listNext.Contains(parBUL)) {
            return;
        }
        listNext.Add(parBUL);
    }

    public void addPrev(buttonUpgradeLeaf parBUL) {
        if (listPrev.Contains(parBUL)) {
            return;
        }
        listPrev.Add(parBUL);
    }

    public void clearNext() {
        listNext.Clear(); 
    }

    public void clearPrev() {
        listPrev.Clear();
    }
    #endregion next_prev

    #region test
    public void testUpgradeCode() {
        Debug.Log("buttonUpgradeLeaf test code : " + thisUpgrade.code);
    }
    #endregion test
}
