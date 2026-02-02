using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class saveComponent {
    public dataSaveBasicMap dataSaveBasicNormal { get; private set; }
    public dataSaveBasicMap dataSaveBasicHard { get; private set; }
    public dataSaveBasicMap dataSaveBasicElite { get; private set; }

    public saveComponent(){
        dataSaveBasicNormal = LOAD<dataSaveBasicMap>("/Save/SaveBasic" + enumMapType.Normal.ToString() + ".json");
        dataSaveBasicHard = LOAD<dataSaveBasicMap>("/Save/SaveBasic" + enumMapType.Hard.ToString() + ".json");
        dataSaveBasicElite = LOAD<dataSaveBasicMap>("/Save/SaveBasic" + enumMapType.Elite.ToString() + ".json");
        dataSaveBasicNormal.ensureSaveBasicValid();
        dataSaveBasicHard.ensureSaveBasicValid();
        dataSaveBasicElite.ensureSaveBasicValid();
    }

    #region Save&Load
    // generic is not necessary but used to ensure parData has only json-convertable fields
    public void SAVE<T>(string parPath, T parData) where T : IDataInsurance {
        gameManager.GM.FC.exportPersistentJson(parPath, parData);
    }

    public T LOAD<T>(string parPath) where T : IDataInsurance,new() {
        fileComponent.ensurePersistentPath(ref parPath);

        // if Save json file doesn't exist, create it
        if (!File.Exists(parPath)) {
            T tempNew = new T();
            tempNew.emergencyInit();
            gameManager.GM.FC.exportPersistentJson(parPath, tempNew);
            return tempNew;
        } else {
            return gameManager.GM.FC.importPersistentJson<T>(parPath);
        }
    }
    #endregion Save&Load

    public void saveMap(enumMapType parEnumMapType) {
        switch (parEnumMapType) {
            case enumMapType.Normal:
                SAVE<dataSaveBasicMap>("/Save/SaveBasic" + parEnumMapType.ToString() + ".json", dataSaveBasicNormal);
                break;
            case enumMapType.Hard:
                SAVE<dataSaveBasicMap>("/Save/SaveBasic" + parEnumMapType.ToString() + ".json", dataSaveBasicHard);
                break;
            case enumMapType.Elite:
                SAVE<dataSaveBasicMap>("/Save/SaveBasic" + parEnumMapType.ToString() + ".json", dataSaveBasicElite);
                break;
            default:
                break;
        }
    }

    public dataSaveBasicMap getDataSaveBasicMap(enumMapType parEnumMapType) {
        return parEnumMapType switch {
            enumMapType.Normal => dataSaveBasicNormal,
            enumMapType.Hard => dataSaveBasicHard,
            enumMapType.Elite => dataSaveBasicElite,
            _ => new dataSaveBasicMap()
        };
    }
}
