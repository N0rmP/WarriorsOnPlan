using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;
using System.Text;

public class upgradeComponent {
    private int stars_ = 0;
    public int stars {
        get {
            return stars_;
        }
        private set {
            stars_ = Math.Max(0, value);
            mapManager.MM.MUC.CU.setTextStarCounter(stars_);
        } 
    }

    private List<upgradeAbst> listUpgradeUndone;
    private List<upgradeAbst> listUpgradeDone;
    public upgradeAbst[] arrUpgradeDone {
        get {
            return listUpgradeDone.ToArray();
        }
    }

    public upgradeComponent() {
        listUpgradeUndone = new List<upgradeAbst>();
        listUpgradeDone = new List<upgradeAbst>();
    }

    #region preparation_initiation
    public void prepareUpgrade(enumMapType parEnumMapType) {
        prepareUpgradeTree(parEnumMapType);
        restoreUpgrade(parEnumMapType);
    }

    // draw total upgrade tree and set all upgrades
    private void prepareUpgradeTree(enumMapType parEnumMapType) {
        dataUpgradeTree tempDataUpgradeTree = gameManager.GM.DHouC.getDataUpgradeTree(parEnumMapType);

        // prepare upgradetree
        foreach (dataUpgradeLeaf dul in tempDataUpgradeTree.ArrUpgradeTreeZero) {
            mapManager.MM.MUC.CU.getBoxUpgradeTree(0).recursiveAddLeaf(dul, 0);
        }
        foreach (dataUpgradeLeaf dul in tempDataUpgradeTree.ArrUpgradeTreeOne) {
            mapManager.MM.MUC.CU.getBoxUpgradeTree(1).recursiveAddLeaf(dul, 0);
        }
        foreach (dataUpgradeLeaf dul in tempDataUpgradeTree.ArrUpgradeTreeTwo) {
            mapManager.MM.MUC.CU.getBoxUpgradeTree(2).recursiveAddLeaf(dul, 0);
        }
    }

    private void restoreUpgrade(enumMapType parEnumMapType) {
        dataSaveBasicMap tempDSBM = gameManager.GM.SaveC.getDataSaveBasicMap(enumMapType.Normal);

        // update reward stars
        int tempStars = 0;
        foreach (int cleared in tempDSBM.getLevelCleared()) {
            tempStars += gameManager.GM.DHouC.getDataLevel(cleared).IsBossLevel ? 2 : 1;
        }
        stars = tempStars;
        stars = 99;     // ★ 테스트용 왕창 별 주기

        // restore upgrade tree, technically it's just restoring UI with dataUpgradeTree
        // all first layer upgrades be frontlined regardless how many upgrades are done
        for (int i = 0; i < 3; i++) {
            mapManager.MM.MUC.CU.getBoxUpgradeTree(i).frontlineFirstUpgrade();
        }
        // actual restore upgrade tree
        foreach (int codeDone in tempDSBM.getUpgradeDone()) {
            mapManager.MM.MUC.CU.getButtonUpgradeLeaf(codeDone).systemDoUpgrade();
        }
    }
    #endregion preparation_initiation

    #region do_undo
    // calling doUpgrade directly can cause UI missing, please call it via buttonUpgradeLeaf
    public void doUpgrade(upgradeAbst parUpgrade, bool parIsSystemic = false) {
        if (!parIsSystemic) {
            if (stars < parUpgrade.starRequired || !listUpgradeUndone.Contains(parUpgrade)) {
                return;
            }
            stars -= parUpgrade.starRequired;
        }

        listUpgradeUndone.Remove(parUpgrade);
        listUpgradeDone.Add(parUpgrade);
    }

    // calling undoUpgrade directly can cause UI missing, please call it via buttonUpgradeLeaf
    public void undoUpgrade(upgradeAbst parUpgrade, bool parIsSystemic = false) {
        if (!parIsSystemic) {
            if (!listUpgradeDone.Contains(parUpgrade)) {
                return;
            }
            stars += parUpgrade.starRequired;
        }

        listUpgradeDone.Remove(parUpgrade);
        listUpgradeUndone.Add(parUpgrade);        
    }

    public void clear() {
        listUpgradeDone.Clear();
        listUpgradeUndone.Clear();
    }
    #endregion do_undo

    #region test
    public void testListUpgradeUndone() {
        StringBuilder tempResult = new StringBuilder("test listUpgradeUndone : ");
        foreach (upgradeAbst ua in listUpgradeUndone) {
            tempResult.Append(ua);
            tempResult.Append(" , ");
        }
        Debug.Log(tempResult);
    }
    public void testListUpgradeDone() {
        StringBuilder tempResult = new StringBuilder("test listUpgradeDone : ");
        foreach (upgradeAbst ua in listUpgradeDone) {
            tempResult.Append(ua);
            tempResult.Append(" , ");
        }
        Debug.Log(tempResult);
    }
    #endregion test
}
