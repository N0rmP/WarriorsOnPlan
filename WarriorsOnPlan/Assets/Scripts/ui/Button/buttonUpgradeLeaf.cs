using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Cases;

// caution : in current buttonUpgradeLeaf system, two parent-nodes can't share one child-node
/*
    BulCode is not coherent with codableObject's code
    BulCode's each digit represents below information
        first digit (highest)       : upgrade tree number (ascending from 1)
        second digit                : layer in the tree (ascending from 1)
        last two digits (lowest)    : numer in the layer (ascending from 1)
*/
public class buttonUpgradeLeaf : MonoBehaviour {
    // upgraded buttonUpgradeLeaf has yellow frame, matUIColorIgnorer helps the frame ignore the sprite's original color and become yellow
    private static Material matUIColorIgnorer = null;

    public int thisBulCode { get; private set; }
    private upgradeAbst thisUpgrade = null;
    // thisLineToUpper is the line connected to the layer lower (upper)
    private line thisLineToUpper = null;
    private List<buttonUpgradeLeaf> listNext;

    public bool isUpgraded {
        get {
            return thisUpgrade.isUpgraded;
        }
    }

    public Action<buttonUpgradeLeaf> init(int parButtonUpgradeLeafCode, upgradeAbst parUpgrade) {
        if (matUIColorIgnorer == null) {
            matUIColorIgnorer = Resources.Load<Material>("Material/materialUIColorIgnorer");
        }
        GetComponent<buttonCustom>().interactable = false;
        if (listNext == null) {
            listNext = new List<buttonUpgradeLeaf>();
        } else {
            foreach (buttonUpgradeLeaf bul in listNext) { 
                
            }
            listNext.Clear();
        }

        thisBulCode = parButtonUpgradeLeafCode;
        thisUpgrade = parUpgrade;
        GetComponent<showerCase>().setCase(thisUpgrade);
        transform.GetChild(0).GetComponent<Image>().sprite = thisUpgrade.caseImage;
        transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = thisUpgrade.starRequired.ToString();
        mapManager.MM.UC.undoUpgrade(thisUpgrade, true);

        return (x) => listNext.Add(x);
    }

    public void replaceLine(boxUpgradeTree parCurTree, line parLineFromUpper) {
        // retrieving & setting thisLineToUpper
        if (parLineFromUpper != null) {
            if (thisLineToUpper != null) {
                gameManager.GM.LC.retrieveLine(thisLineToUpper);
            }
            thisLineToUpper = parLineFromUpper;
            thisLineToUpper.transform.SetAsFirstSibling();
        }

        // replace new lines to next buttons, and replacing lines from next buttons
        foreach (buttonUpgradeLeaf bul in listNext) {
            bul.replaceLine(
                parCurTree,
                gameManager.GM.LC.placeLine(parCurTree.GetComponent<RectTransform>(), GetComponent<RectTransform>().position, bul.GetComponent<RectTransform>().position)
            );
        }
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

    #region click
    public void click() {
        doUpgrade();

        // data revision & save
        gameManager.GM.SaveC.getDataSaveBasicMap(gameManager.GM.curMapType).addUpgradeDone(
                mapManager.MM.MUC.CU.getButtonUpgradeLeaf(thisBulCode).thisBulCode
        );
        gameManager.GM.SaveC.SAVE();
    }

    public void rightClick() {
        undoUpgrade();

        // data revision & save
        gameManager.GM.SaveC.getDataSaveBasicMap(gameManager.GM.curMapType).removeUpgradeDone(
                mapManager.MM.MUC.CU.getButtonUpgradeLeaf(thisBulCode).thisBulCode
        );
        gameManager.GM.SaveC.SAVE();
    }
    #endregion click

    #region upgrade_method
    // frontline means upgrade-available, prev-upgrade is done and this upgrade is opened
    private void frontlineUpgrade() {
        // frontlining fails when thisUpgrade is done, please call this method through undoUpgrade to solve this problem
        if (thisUpgrade.isUpgraded) {
            return;
        }

        setFrameImage(Color.white, false);
        GetComponent<buttonCustom>().interactable = true;
    }

    public void doUpgrade() {
        if (thisUpgrade.isUpgraded || !GetComponent<buttonCustom>().interactable) {
            return;
        }
        
        if (mapManager.MM.UC.stars < thisUpgrade.starRequired) {
            gameManager.GM.PC.popupBasicAlert(RectTransformUtility.WorldToScreenPoint(null, GetComponent<RectTransform>().position + new Vector3(0f, 50f, 0f)), "test : not enough stars");
            return;
        }

        thisUpgrade.doUpgrade();        

        // make imageFrame Ignores original color, and set its color
        setFrameImage(Color.yellow, true);        

        foreach (buttonUpgradeLeaf bul in listNext) {
            bul.frontlineUpgrade();
        }
    }

    // systemDoUpgrade is ensured to do upgrade, is makes this BUL.interactable and calls doUpgrade
    public void systemDoUpgrade() {
        GetComponent<buttonCustom>().interactable = true;
        doUpgrade();
    }

    // undoUpgrade is for public calling and contains frontlining this buttonUpgradeLeaf, please use undoUpgradeChian for what you think
    public void undoUpgrade() {
        if (!thisUpgrade.isUpgraded) {
            return;
        }

        undoUpgradeChain();
        frontlineUpgrade();
    }

    // undoUpgrade all next upgrades, it doesn't contain frontlining
    private void undoUpgradeChain() {
        thisUpgrade.undoUpgrade();        

        setFrameImage(Color.white, false);
        GetComponent<buttonCustom>().interactable = false;

        foreach (buttonUpgradeLeaf bul in listNext) {
            bul.undoUpgradeChain();
        }        
    }
    #endregion upgrade_method

    #region test
    public void testUpgradeCode() {
        Debug.Log("buttonUpgradeLeaf test code : " + thisUpgrade.code);
    }
    #endregion test
}
