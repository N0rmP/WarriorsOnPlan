using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class dataHouseComponent {
    private List<dataLevel> listLevelNormal;
    private List<dataLevel> listLevelHard;
    private List<dataLevel> listLevelElite;
    private List<dataLevel> listLevelTest;

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
        listLevelNormal = gameManager.GM.FC.importResourcesJsonArr<dataLevel>("Level/Normal", false).ToList<dataLevel>();
        listLevelHard = gameManager.GM.FC.importResourcesJsonArr<dataLevel>("Level/Hard", false).ToList<dataLevel>();
        listLevelElite = gameManager.GM.FC.importResourcesJsonArr<dataLevel>("Level/Elite", false).ToList<dataLevel>();
        listLevelTest = gameManager.GM.FC.importResourcesJsonArr<dataLevel>("Level/Test", false).ToList<dataLevel>();
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
    public dataLevel[] getArrLevel(enumMapType parMapType) {
        return (parMapType switch {
            enumMapType.Normal => listLevelNormal,
            enumMapType.Hard => listLevelHard,
            enumMapType.Elite => listLevelElite,
            enumMapType.Test => listLevelTest,
            _ => new List<dataLevel>()
        }).ToArray();
    }

    public dataLevel getDataLevel(int parMapCode) {
        enumMapType tempEnumMapType = (parMapCode / 10000) switch {
            1 => enumMapType.Normal,
            2 => enumMapType.Hard,
            3 => enumMapType.Elite,
            9 or _ => enumMapType.Test
        };

        foreach (dataLevel dl in getArrLevel(tempEnumMapType)) {
            if (parMapCode == dl.LevelCode) {
                return dl;
            }
        }

        Debug.Log("there is no level data of MapCode \'" + parMapCode + "\'");
        return listLevelTest[0];
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
