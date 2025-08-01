using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum enumMapType {
    None = -9,
    Normal = 1,
    Hard = 2,
    Elite = 3,
    Test = 9,
}

public class mapManager : MonoBehaviour {
    public static mapManager MM = null;    

    public mapUIComponent MUC { get; private set; }
    public upgradeComponent UC { get; private set; }


    public void Awake() {
        if (MM == null) {
            MM = this;
        } else {
            Destroy(this);
        }
        
        MUC = new mapUIComponent();
        UC = new upgradeComponent();
    }

    // prepareMap uses enumMapType by argument not by gameManager.GM.curMapType, it ensures some flexible call
    public void prepareMap(enumMapType parEnumMapType) {
        restoreMap(parEnumMapType);

        // ★ 테스트용 임시 호출, 추후 메인메뉴에서 게임시작 버튼 누를 때 난이도에 따라 호출하게 하기
        UC.prepareUpgrade(parEnumMapType);
    }

    #region restore
    private void restoreMap(enumMapType parEnumMapType) {
        dataSaveBasicMap tempDSBM = gameManager.GM.SaveC.getDataSaveBasicMap(parEnumMapType);
        restoreLevel(parEnumMapType, tempDSBM);
    }

    private void restoreLevel(enumMapType parEnumMapType, dataSaveBasicMap parDataSaveBasicMap) {
        // tempSetUncleared contains the next uncleared Levels
        List<int> tempListUncleared = new List<int>();

        // restore cleared levels, and find the front-end levels
        MUC.clearButtonLevel();
        foreach (int cleared in parDataSaveBasicMap.getLevelCleared()) {
            tempListUncleared.Remove(cleared);
            foreach (int next in gameManager.GM.DHouC.getDataLevel(cleared).NextLevelCode) {
                if (tempListUncleared.Contains(next)) { 
                    continue; 
                }
                tempListUncleared.Add(next);
            }
            MUC.prepareButtonLevel(cleared, true);
        }
        
        // restore the next uncleared levels
        if (parDataSaveBasicMap.getLevelCleared().Length == 0) {
            MUC.prepareButtonLevel((int)parEnumMapType * 10000 + 0101, false);
        } else {
            foreach (int frontline in tempListUncleared) {
                MUC.prepareButtonLevel(frontline, false);
            }
        }
    }
    #endregion restore
}