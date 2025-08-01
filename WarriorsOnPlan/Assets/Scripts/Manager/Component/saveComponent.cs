using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class saveComponent {
    public dataSaveBasicMap dataSaveBasicNormal { get; private set; }
    public dataSaveBasicMap dataSaveBasicHard { get; private set; }
    public dataSaveBasicMap dataSaveBasicElite { get; private set; }

    public saveComponent(){
        dataSaveBasicMap importDataSaveBasicMap(enumMapType parEnumMapType) {
            // if Save directory doesn't exist, create it
            if (!Directory.Exists(Application.persistentDataPath + "/Save")) {                
                Directory.CreateDirectory(Application.persistentDataPath + "/Save");
            }

            // if Save json file doesn't exist, create it
            if (!File.Exists(Application.persistentDataPath + getSavePath(parEnumMapType) + ".json")) {
                dataSaveBasicMap tempNew = new dataSaveBasicMap();
                tempNew.emergencyInit();
                File.WriteAllText(
                    Application.persistentDataPath + getSavePath(parEnumMapType) + ".json", JsonConvert.SerializeObject(tempNew, Formatting.Indented)
                );
                return tempNew;
            } else {
                return gameManager.GM.FC.importPersistentJson<dataSaveBasicMap>(getSavePath(parEnumMapType));
            }
        }
        
        dataSaveBasicNormal = importDataSaveBasicMap(enumMapType.Normal);        
        dataSaveBasicHard = importDataSaveBasicMap(enumMapType.Hard);
        dataSaveBasicElite = importDataSaveBasicMap(enumMapType.Elite);
    }

    public dataSaveBasicMap loadBasicMap(enumMapType parEnumMapType) {
        return gameManager.GM.FC.importPersistentJson<dataSaveBasicMap>(getSavePath(parEnumMapType));
    }

    public void SAVE() {
        // save dataSaveBasicMap 
        File.WriteAllText(Application.persistentDataPath + getSavePath(enumMapType.Normal) + ".json", JsonConvert.SerializeObject(dataSaveBasicNormal, Formatting.Indented));
        File.WriteAllText(Application.persistentDataPath + getSavePath(enumMapType.Hard) + ".json", JsonConvert.SerializeObject(dataSaveBasicHard, Formatting.Indented));
        File.WriteAllText(Application.persistentDataPath + getSavePath(enumMapType.Elite) + ".json", JsonConvert.SerializeObject(dataSaveBasicElite, Formatting.Indented));
        
        
    }

    private string getSavePath(enumMapType parEnumMapType) {
        return "/Save/SaveBasic" + parEnumMapType.ToString();
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
