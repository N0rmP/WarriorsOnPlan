using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class mapperBasic : IMapper {

    #region interface_implement
    public void prepareMap() {
        restoreMap(gameManager.GM.curMapType);
        mapManager.MM.UC.prepareUpgradeTree(gameManager.GM.DHouC.getDataUpgradeTree(gameManager.GM.curMapType));
        mapManager.MM.UC.restoreUpgrade(gameManager.GM.SaveC.getDataSaveBasicMap(gameManager.GM.curMapType));
    }

    public void doWhenCombatVictory() {
        gameManager.GM.SaveC.getDataSaveBasicMap(gameManager.GM.curMapType).addLevelCleared(combatManager.CM.curDataLevel.LevelCode);
        gameManager.GM.SaveC.getDataSaveBasicMap(gameManager.GM.curMapType).addStars(combatManager.CM.curDataLevel.IsBossLevel ? 2 : 1);
        gameManager.GM.SaveC.saveMap(gameManager.GM.curMapType);
    }

    public void doWhenCombatDefeated() { }

    public void save() {
        throw new System.NotImplementedException();
    }
    #endregion interface_implement

    #region restore
    private void restoreMap(enumMapType parEnumMapType) {
        dataSaveBasicMap tempDSBM = gameManager.GM.SaveC.getDataSaveBasicMap(parEnumMapType);
        restoreLevel(parEnumMapType, tempDSBM);
    }

    private void restoreLevel(enumMapType parEnumMapType, dataSaveBasicMap parDataSaveBasicMap) {
        // tempListFrontline contains the next uncleared Levels
        List<int> tempListFrontline = new List<int>();

        // restore cleared levels, and find the front-end levels
        mapManager.MM.MUC.clearButtonLevel();
        foreach (int cleared in parDataSaveBasicMap.getLevelCleared()) {
            tempListFrontline.Remove(cleared);
            foreach (int next in gameManager.GM.DHouC.getDataLevel(cleared).NextLevelCode) {
                if (tempListFrontline.Contains(next)) { 
                    continue; 
                }
                tempListFrontline.Add(next);
            }
            mapManager.MM.MUC.prepareButtonLevel(cleared, true);
        }
        
        // restore the next uncleared levels
        if (parDataSaveBasicMap.getLevelCleared().Length == 0) {
            mapManager.MM.MUC.prepareButtonLevel((int)parEnumMapType * 10000 + 0101, false);
        } else {
            foreach (int frontline in tempListFrontline) {
                mapManager.MM.MUC.prepareButtonLevel(frontline, false);
            }
        }
    }    
    #endregion restore
}