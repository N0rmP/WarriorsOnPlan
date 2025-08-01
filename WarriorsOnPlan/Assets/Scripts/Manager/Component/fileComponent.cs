using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Newtonsoft;
using Newtonsoft.Json;
using System.Security.Policy;
using Cases;
using Newtonsoft.Json.Converters;

public class fileComponent {
    #region Resources
    public T importResourcesJson<T>(string parHalfPath, bool isTranslationRequired = true) where T : IDataInsurance {
        T tempResult;
        try {
            tempResult = JsonConvert.DeserializeObject<T>(Resources.Load<TextAsset>(getResourcesPath(parHalfPath, isTranslationRequired)).ToString());
        } catch (Exception e) {
            Debug.Log("importResourcesJson failed to deserialize " + getResourcesPath(parHalfPath, isTranslationRequired) + " ((" + e);
            tempResult = default(T);
            tempResult.emergencyInit();
        }

        return tempResult;
    }

    public T[] importResourcesJsonArr<T>(string parPath, bool isTranslationRequired = true) where T : struct, IDataInsurance {
        TextAsset[] tempTextAssetArr = Resources.LoadAll<TextAsset>(getResourcesPath(parPath, isTranslationRequired));
        T[] tempResult = new T[tempTextAssetArr.Length];
        for (int i = 0; i < tempResult.Length; i++) {
            try {
                tempResult[i] = JsonConvert.DeserializeObject<T>(tempTextAssetArr[i].ToString());
            } catch (Exception e) {
                Debug.Log("importResourcesJsonArr failed to deserialize " + tempTextAssetArr[i].name + " from " + getResourcesPath(parPath, isTranslationRequired) + " ((" + e);
                tempResult[i] = default(T);
                tempResult[i].emergencyInit();
            }
        }

        return tempResult;
    }

    private string getResourcesPath(string parHalfPath, bool isTranslationRequired = true) {
        return "Database/" + (isTranslationRequired ? gameManager.GM.option.curTranslation.ToString() + "/" : "") + parHalfPath;
    }
    #endregion Resources

    #region persistentDataPath
    // parPath should include file-extension
    public T importPersistentJson<T>(string parPath) where T : struct, IDataInsurance {
        string tempPath = getPersistentPath(parPath);
        if (!File.Exists(tempPath)) {
            Debug.Log("fileComponent.importPersistentJson results in error with path " + tempPath);
            T tempResult = default(T);
            tempResult.emergencyInit();
            return tempResult;
        }

        return JsonConvert.DeserializeObject<T>(File.ReadAllText(tempPath));
    }

    // exportJson exports only IDataInsurance object, only to Application.persistent
    public void exportPersistentJson(string parPath, IDataInsurance parData) {
        string tempPath = getPersistentPath(parPath);
        File.WriteAllText(tempPath, JsonConvert.SerializeObject(parData, Formatting.Indented));
    }

    private string getPersistentPath(string parHalfPath) {
        return Application.persistentDataPath + parHalfPath + ".json";
    }
    #endregion persistentDataPath
}
