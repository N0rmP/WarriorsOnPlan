using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class canvasUpgrade : MonoBehaviour {
    private boxUpgradeTree[] boxEachTree;
    // buttonUpgradeLeaf's code is different from upgrade-code, it consists of (tree number one digit) + (layer one digit) + (numer in layer two digit)
    private Dictionary<int, buttonUpgradeLeaf> dictCodeButtonUpgradeLeaf;
    private TextMeshProUGUI textStarCounter;

    private carrierGeneric<buttonUpgradeLeaf> carrierBUL;

    public void Awake() {
        boxEachTree = new boxUpgradeTree[3];
        boxEachTree[0] = transform.Find("boxTreeZero").GetComponent<boxUpgradeTree>();
        boxEachTree[1] = transform.Find("boxTreeOne").GetComponent<boxUpgradeTree>();
        boxEachTree[2] = transform.Find("boxTreeTwo").GetComponent<boxUpgradeTree>();
        dictCodeButtonUpgradeLeaf = new Dictionary<int, buttonUpgradeLeaf>();
        textStarCounter = transform.FindThoroughly("textStarCounter").GetComponent<TextMeshProUGUI>();

        GameObject tempPrefabButtonUpgradeLeaf = Resources.Load<GameObject>("Prefab/UI/Upgrade/buttonUpgradeLeaf");
        carrierBUL = new carrierGeneric<buttonUpgradeLeaf>(
            () => {
                return Instantiate(tempPrefabButtonUpgradeLeaf).GetComponent<buttonUpgradeLeaf>();
            },
            (x) => {
                x.transform.SetParent(null);
                x.gameObject.SetActive(false);
            }
            );
    }

    public void addButtonUpgradeLeaf(buttonUpgradeLeaf parBUL) {
        if (dictCodeButtonUpgradeLeaf.ContainsKey(parBUL.thisBulCode)) {
            Debug.Log("canvasUpgrade.addButtonUpgradeLeaf results in an error due to parBUL.thisBulCode fold : " + parBUL.thisBulCode);
            return;
        }

        dictCodeButtonUpgradeLeaf.Add(parBUL.thisBulCode, parBUL);
    }

    public void undoAllUpgrade() {
        for (int i = 0; i < 3; i++) {
            boxEachTree[i].undoFirstUpgrade();
        }
    }

    public void setTextStarCounter(int parStarAmount) {
        textStarCounter.text = parStarAmount.ToString();
    }

    public buttonUpgradeLeaf createBUL() {
        return carrierBUL.getInterceptor();
    }

    #region get
    public boxUpgradeTree getBoxUpgradeTree(int parCategory) {
        return boxEachTree[parCategory];
    }

    public buttonUpgradeLeaf getButtonUpgradeLeaf(int parBulCode) {
        if (!dictCodeButtonUpgradeLeaf.ContainsKey(parBulCode)) {
            Debug.Log("canvasUpgrade.getButtonUpgradeLeaf failed due to key-not-existence : " + parBulCode);
            return null;
        }

        return dictCodeButtonUpgradeLeaf[parBulCode];
    }

    public IEnumerable<buttonUpgradeLeaf> getButtonUpgradeLeafTotal() {
        return dictCodeButtonUpgradeLeaf.Values;
    }
    #endregion get
}
