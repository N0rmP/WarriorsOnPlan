using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cases;
using UnityEngine.Assertions.Must;

public class boxUpgradeTree : MonoBehaviour {
    [SerializeField]
    private int treeNumber;

    private GameObject prefabLayer;
    private GameObject prefabButtonUpgradeLeaf;

    private Transform boxTree;
    private List<Transform> boxLayer;

    public void Awake() {
        boxTree = transform.GetChild(0);
        boxLayer = new List<Transform>();

        prefabLayer = Resources.Load<GameObject>("Prefab/UI/Upgrade/boxTreeLayer");
    }

    public void Update() {
        // ¡Ú
        if (Input.GetKeyDown(KeyCode.Space)) {
            foreach (Transform tr in boxLayer) {
                foreach (Transform obj in tr) {
                    Debug.Log(obj.gameObject + " :" + obj.GetComponent<RectTransform>().localPosition + " / " + obj.GetComponent<RectTransform>().anchoredPosition + " / " + obj.GetComponent<RectTransform>().position);
                }
            }
        }
    }

    // parIsFirstEnter is used to place lines, it is not recommended to set it manually because it can cause misplacing lines
    public buttonUpgradeLeaf recursiveAddLeaf(dataUpgradeLeaf parDUL, int parLayer, bool parIsFirstEnter = true) {
        // disalble all layers at first recursion
        if (parIsFirstEnter) {
            foreach (Transform tr in boxLayer) {
                tr.gameObject.SetActive(false);
            }
        }

        // add layer until it reaches parLayer, and ensure the layer enabled
        while (boxLayer.Count <= parLayer) {
            makeLayer();
        }
        boxLayer[parLayer].gameObject.SetActive(true);

        // prepare new buttonUpgradeLeaf
        buttonUpgradeLeaf tempCurBUL = mapManager.MM.MUC.CU.createBUL();
        tempCurBUL.gameObject.SetActive(true);
        tempCurBUL.transform.SetParent(boxLayer[parLayer]);

        // initiate buttonUpgradeLeaf, and get delegate for setting its listNext
        Action<buttonUpgradeLeaf> tempDelSetNext = tempCurBUL.init(
            (treeNumber + 1) * 1000 + (parLayer + 1) * 100 + (tempCurBUL.transform.GetSiblingIndex() + 1),
            gameManager.GM.MC.makeCodableObject<upgradeAbst>(parDUL.code, parDUL.parameters, null)
        );
        // buttonUpgradeLeaf's code is different from upgrade-code, it consists of (tree number one digit) + (layer one digit) + (numer in layer two digit)
        mapManager.MM.MUC.CU.addButtonUpgradeLeaf(tempCurBUL);

        // set listNext of cur-created buttonUpgradeLeaf
        buttonUpgradeLeaf tempNextBUL;
        foreach (dataUpgradeLeaf dul in parDUL.next) {
            tempNextBUL = recursiveAddLeaf(dul, parLayer + 1, false);
            tempDelSetNext(tempNextBUL);            
        }

        if (parIsFirstEnter) {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetChild(0).GetComponent<RectTransform>());
            tempCurBUL.replaceLine(this, null);
        }

        return tempCurBUL;
    }

    private void makeLayer() {
        boxLayer.Add(GameObject.Instantiate(prefabLayer, transform.GetChild(0)).transform);
    }

    #region first_layer
    // frontline upgrades in the first layer(root nodes)
    public void frontlineFirstUpgrade() {
        if (boxLayer.Count == 0) {
            return;
        }
        
        foreach (Transform transformFirstUpgrade in boxLayer[0]) {
            transformFirstUpgrade.GetComponent<buttonUpgradeLeaf>().undoUpgrade();
            //transformFirstUpgrade.GetComponent<buttonUpgradeLeaf>().frontlineUpgrade();
        }
    }

    // undo upgrades in the first layer(root nodes), it means cancel all upgrades in this tree
    public void undoFirstUpgrade() {
        if (boxLayer.Count == 0) {
            return;
        }

        foreach (Transform transformFirstUpgrade in boxLayer[0]) {
            transformFirstUpgrade.GetComponent<buttonUpgradeLeaf>().undoUpgrade();
        }
    }
    #endregion first_layer
}
