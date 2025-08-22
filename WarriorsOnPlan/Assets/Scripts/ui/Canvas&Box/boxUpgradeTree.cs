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

    private static GameObject prefabLayer = null;

    private List<Transform> listLayer;
    private List<line> listLine;

    public void Awake() {
        if (prefabLayer == null) {
            prefabLayer = Resources.Load<GameObject>("Prefab/UI/Upgrade/boxTreeLayer");
        }

        listLayer = new List<Transform>();
        listLine = new List<line>();
    }

    /*
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
    */

    public void prepareUpgradeTree(dataUpgradeLeaf[] parArrDUL, dataUpgradeTreeEdge[] parArrEdge) {
        // disable this tree first
        buttonUpgradeLeaf tempToBeRetrieved;
        foreach (Transform tr in listLayer) {
            foreach (Transform ttr in tr) {
                if (ttr.TryGetComponent<buttonUpgradeLeaf>(out tempToBeRetrieved)) {
                    mapManager.MM.MUC.CU.returnBUL(tempToBeRetrieved);
                }
            }
            tr.gameObject.SetActive(false);
        }

        // prepare all buttonUpgradeLeaf
        foreach (dataUpgradeLeaf dul in parArrDUL) {
            // second digit of dul.LeafCode represents layer-index, ensure the adequate layer is activated or created
            int tempLayerIndex = (dul.LeafCode / 100) % 10 - 1;
            while (listLayer.Count < tempLayerIndex + 1) {
                makeLayer();
            }
            for (int i = 0; i <= tempLayerIndex; i++) {
                listLayer[i].gameObject.SetActive(true);
            }

            // prepare one new buttonUpgradeLeaf
                // get or create new buttonUpgradeLeaf
            buttonUpgradeLeaf tempCurBUL = mapManager.MM.MUC.CU.createBUL();
            tempCurBUL.gameObject.SetActive(true);
            tempCurBUL.transform.SetParent(listLayer[tempLayerIndex]);
            tempCurBUL.transform.localScale = new Vector3(1f, 1f, 1f);
                // init new buttonUpgradeLeaf
            tempCurBUL.init(dul.LeafCode, gameManager.GM.MC.makeCodableObject<upgradeAbst>(dul.UpgradeCode, dul.Parameters, null));
            mapManager.MM.MUC.CU.addButtonUpgradeLeaf(tempCurBUL);
        }

        // prepare to prepare all edges
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetChild(0).GetComponent<RectTransform>());
        foreach (line l in listLine) {
            gameManager.GM.LC.retrieveLine(l);        
        }
        listLine.Clear();
        // prepare all edges
        buttonUpgradeLeaf tempParent;
        buttonUpgradeLeaf tempChild;
        line tempLine;
        foreach (dataUpgradeTreeEdge edge in parArrEdge) {
            tempParent = mapManager.MM.MUC.CU.getButtonUpgradeLeaf(edge.parent);
            tempChild = mapManager.MM.MUC.CU.getButtonUpgradeLeaf(edge.child);
            tempParent.addNext(tempChild);
            tempChild.addPrev(tempParent);

            tempLine = gameManager.GM.LC.placeLine(
                GetComponent<RectTransform>(),
                tempParent.GetComponent<RectTransform>().convertAcrossRect(GetComponent<RectTransform>(), Vector3.zero),
                tempChild.GetComponent<RectTransform>().convertAcrossRect(GetComponent<RectTransform>(), Vector3.zero)
            );
            listLine.Add(tempLine);
            tempLine.transform.SetAsFirstSibling();
        }
    }

    private void makeLayer() {
        listLayer.Add(GameObject.Instantiate(prefabLayer, transform.GetChild(0)).transform);
    }

    #region first_layer
    // frontline upgrades in the first layer(root nodes)
    public void frontlineFirstUpgrade() {
        if (listLayer.Count == 0) {
            return;
        }
        
        foreach (Transform transformFirstUpgrade in listLayer[0]) {
            // transformFirstUpgrade.GetComponent<buttonUpgradeLeaf>().undoUpgrade();
            transformFirstUpgrade.GetComponent<buttonUpgradeLeaf>().paintFrontline();
        }
    }

    // undo upgrades in the first layer(root nodes), it means cancel all upgrades in this tree
    public void undoFirstUpgrade() {
        if (listLayer.Count == 0) {
            return;
        }

        foreach (Transform transformFirstUpgrade in listLayer[0]) {
            transformFirstUpgrade.GetComponent<buttonUpgradeLeaf>().rightClick();
        }
    }
    #endregion first_layer
}
