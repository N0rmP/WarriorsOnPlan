using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class excelComponent
{
    private Dictionary<string, dataWeaponEntity> dictTestWeapons;

    public excelComponent() {
        dictTestWeapons = getExcelWeapon();
    }

    public Dictionary<string, dataWeaponEntity> getExcelWeapon() {
        Dictionary<string, dataWeaponEntity> tempResult = new Dictionary<string, dataWeaponEntity>();
        excel_weapon tempEW = Resources.Load<excel_weapon>("Database/excel_weapon");

        foreach (dataWeaponEntity DWE in tempEW.Sheet1) {
            tempResult.Add(DWE.Name, DWE);
        }

        return tempResult;
    }

    #region search
    public dataWeaponEntity getWeaponEntiy(string parName) {
        return dictTestWeapons[parName];
    }
    #endregion search
}
