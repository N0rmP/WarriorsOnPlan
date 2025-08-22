using System;
//using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class dataHouseComponent {
    private Dictionary<int, dataLevel> dictLevelNormal;
    private Dictionary<int, dataLevel> dictLevelHard;
    private Dictionary<int, dataLevel> dictLevelElite;
    private Dictionary<int, dataLevel> dictLevelTest;

    private dataUpgradeTree dataUpgradeTreeNormal;
    private dataUpgradeTree dataUpgradeTreeHard;
    private dataUpgradeTree dataUpgradeTreeElite;

    public dataBookWords bookWords { get; private set; }
    public dataBookConfirmQuestion bookConfirmQuestion { get; private set; }
    public dataBookPopupAlert bookPopupAlert { get; private set; }
    public dataBookCombatResult bookCombatResult { get; private set; }

    public dataHouseComponent() {
        prepareDataLevel();
        prepareDataUpgradeTree();
        prepareBook();
    }

    #region prepare
    private void prepareDataLevel() {
        Dictionary<int, dataLevel> makeInDictionary(IEnumerable<dataLevel> parCol) {
            Dictionary<int, dataLevel> tempResult = new Dictionary<int, dataLevel>();
            foreach (dataLevel dl in parCol) {
                if (!tempResult.ContainsKey(dl.LevelCode)) {
                    tempResult.Add(dl.LevelCode, dl);
                }
            }
            return tempResult;
        }

        dictLevelNormal = makeInDictionary(gameManager.GM.FC.importResourcesJsonArr<dataLevel>("Level/Normal", false).ToList<dataLevel>());
        dictLevelHard = makeInDictionary(gameManager.GM.FC.importResourcesJsonArr<dataLevel>("Level/Hard", false).ToList<dataLevel>());
        dictLevelElite = makeInDictionary(gameManager.GM.FC.importResourcesJsonArr<dataLevel>("Level/Elite", false).ToList<dataLevel>());
        dictLevelTest = makeInDictionary(gameManager.GM.FC.importResourcesJsonArr<dataLevel>("Level/Test", false).ToList<dataLevel>());
    }

    private void prepareDataUpgradeTree() {
        dataUpgradeTreeNormal = gameManager.GM.FC.importResourcesJson<dataUpgradeTree>("General/UpgradeTreeNormal", false);
        dataUpgradeTreeHard = gameManager.GM.FC.importResourcesJson<dataUpgradeTree>("General/UpgradeTreeHard", false);
        dataUpgradeTreeElite = gameManager.GM.FC.importResourcesJson<dataUpgradeTree>("General/UpgradeTreeElite", false);
    }

    // prepareBook could be called outside when translation changes
    public void prepareBook() { 
        bookWords = gameManager.GM.FC.importResourcesJson<dataBookWords>("JustText/BasicWord");
        bookConfirmQuestion = gameManager.GM.FC.importResourcesJson<dataBookConfirmQuestion>("JustText/ConfirmQuestion");
        bookPopupAlert = gameManager.GM.FC.importResourcesJson<dataBookPopupAlert>("JustText/PopupAlert");
        bookCombatResult = gameManager.GM.FC.importResourcesJson<dataBookCombatResult>("JustText/CombatResult");
    }
    #endregion prepare

    #region dataLevel_Managemenet
    public Dictionary<int, dataLevel> getDictLevel(enumMapType parMapType) {
        return parMapType switch {
            enumMapType.Normal => dictLevelNormal,
            enumMapType.Hard => dictLevelHard,
            enumMapType.Elite => dictLevelElite,
            enumMapType.Test => dictLevelTest,
            _ => new Dictionary<int, dataLevel>()
        };
    }

    public dataLevel getDataLevel(int parLevelCode) {
        enumMapType tempEnumMapType = (parLevelCode / 10000) switch {
            1 => enumMapType.Normal,
            2 => enumMapType.Hard,
            3 => enumMapType.Elite,
            9 or _ => enumMapType.Test
        };
        
        Dictionary<int, dataLevel> tempDict = getDictLevel(tempEnumMapType);
        if (tempDict.ContainsKey(parLevelCode)) {
            return tempDict[parLevelCode];
        } else {
            return dictLevelTest[90101];
        }
    }
    #endregion dataLevel_Managemenet

    #region dataUpgradeTree_Management
    public dataUpgradeTree getDataUpgradeTree(enumMapType parEnumMapType) {
        return parEnumMapType switch {
            enumMapType.Normal => dataUpgradeTreeNormal,
            enumMapType.Hard => dataUpgradeTreeHard,
            enumMapType.Elite => dataUpgradeTreeElite,
            _ => new dataUpgradeTree()
        };
    }
    #endregion dataUpgradeTree_Management
}
