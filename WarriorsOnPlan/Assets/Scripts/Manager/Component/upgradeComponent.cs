using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;
using System.Text;
using Unity.VisualScripting;

public class upgradeComponent {
    private List<upgradeAbst> listUpgradeDoneTrue;
    public upgradeAbst[] arrUpgradeDoneTrue {
        get {
            return listUpgradeDoneTrue.ToArray();
        }
    }

    private int starTemporary_ = 0;
    public int starTemporary {
        get {
            return starTemporary_;
        }
        private set {
            starTemporary_ = Math.Max(0, value);
            mapManager.MM.MUC.CU.setTextStarCounter(starTemporary_);
        }
    }
    // key = LeafCode, value = done upgrade itself
    private SortedList<int, upgradeAbst> slUpgradeDoneTemporary;

    public upgradeComponent() {
        listUpgradeDoneTrue = new List<upgradeAbst>();
        slUpgradeDoneTemporary = new SortedList<int, upgradeAbst>();
    }

    // draw total upgrade tree and set all upgrades
    public void prepareUpgradeTree(dataUpgradeTree parDUT) {
        // prepare upgradetree
        mapManager.MM.MUC.CU.getBoxUpgradeTree(0).prepareUpgradeTree(
            parDUT.ArrUpgradeTreeZero,
            parDUT.ArrUpgradeTreeEdgeZero
        );
        mapManager.MM.MUC.CU.getBoxUpgradeTree(1).prepareUpgradeTree(
            parDUT.ArrUpgradeTreeOne,
            parDUT.ArrUpgradeTreeEdgeOne
        );
        mapManager.MM.MUC.CU.getBoxUpgradeTree(2).prepareUpgradeTree(
            parDUT.ArrUpgradeTreeTwo,
            parDUT.ArrUpgradeTreeEdgeTwo
        );
    }

    public void restoreUpgrade(dataSaveBasicMap parDSBM) {
        // all first layer upgrades be frontlined regardless how many upgrades are done
        for (int i = 0; i < 3; i++) {
            mapManager.MM.MUC.CU.getBoxUpgradeTree(i).frontlineFirstUpgrade();
        }

        // actual restore upgrade
        slUpgradeDoneTemporary.Clear();
        foreach (int codeDone in parDSBM.getUpgradeDone()) {
            mapManager.MM.MUC.CU.getButtonUpgradeLeaf(codeDone).systemDoUpgradeTemporary();
        }
        confirmUpgrade();

        // update stars
        starTemporary = parDSBM.stars;
        starTemporary = 99;     // ★ 테스트용 왕창 별 주기, 추후 이 코드 삭제하기
    }

    // store all upgrades from listUpgradeDoneTemporary into listUpgradeDoneTrue
    public void confirmUpgrade() {
        listUpgradeDoneTrue.Clear();
        listUpgradeDoneTrue.AddRange(slUpgradeDoneTemporary.Values);
    }

    // save current upgrade state
    public void saveUpgrade() {
        gameManager.GM.SaveC.getDataSaveBasicMap(gameManager.GM.curMapType).clearUpgradeDone();
        gameManager.GM.SaveC.getDataSaveBasicMap(gameManager.GM.curMapType).addUpgradeDoneRange(slUpgradeDoneTemporary.Keys);
        gameManager.GM.SaveC.getDataSaveBasicMap(gameManager.GM.curMapType).setStars(starTemporary);

        gameManager.GM.SaveC.saveMap(gameManager.GM.curMapType);
    }

    #region true_do
    public void doUpgradeTrue(upgradeAbst parUpgrade) {
        listUpgradeDoneTrue.Add(parUpgrade);
    }

    public void undoUpgradeTrue(upgradeAbst parUpgrade) {
        listUpgradeDoneTrue.Remove(parUpgrade);
    }
    #endregion true_do

    #region temporary_do
    // calling doUpgradeTemporary directly can cause UI missing, please call it via buttonUpgradeLeaf
    public void doUpgradeTemporay(int parLeafCode, upgradeAbst parUpgrade, bool parIsSystemic = false) {
        if (!parIsSystemic) {
            if (starTemporary < parUpgrade.starRequired || slUpgradeDoneTemporary.ContainsKey(parLeafCode)) {
                return;
            }
            starTemporary -= parUpgrade.starRequired;
        }
        slUpgradeDoneTemporary.Add(parLeafCode, parUpgrade);
    }

    // calling undoUpgradeTemporary directly can cause UI missing, please call it via buttonUpgradeLeaf
    public void undoUpgradeTemporary(int parLeafCode, bool parIsSystemic = false) {
        if (!parIsSystemic) {
            if (!slUpgradeDoneTemporary.ContainsKey(parLeafCode)) {
                return;
            }
            starTemporary += slUpgradeDoneTemporary[parLeafCode].starRequired;
        }
        slUpgradeDoneTemporary.Remove(parLeafCode);     
    }
    #endregion temporary

    #region test
    public void testListUpgradeDoneTrue() {
        StringBuilder tempResult = new StringBuilder("test listUpgradeDoneTrue : ");
        foreach (upgradeAbst ua in listUpgradeDoneTrue) {
            tempResult.Append(ua);
            tempResult.Append(", ");
        }
        Debug.Log(tempResult);
    }

    public void testListUpgradeDoneTemporary() {
        StringBuilder tempResult = new StringBuilder("test listUpgradeDoneTemporary : ");
        foreach (int leafcode in slUpgradeDoneTemporary.Keys) {
            tempResult.Append("(");
            tempResult.Append(leafcode);
            tempResult.Append(",");
            tempResult.Append(slUpgradeDoneTemporary[leafcode]);
            tempResult.Append("), ");
        }
        Debug.Log(tempResult);
    }
    #endregion test
}
