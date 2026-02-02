using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text.Json;
using UnityEngine;
/*
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Cases;
using System.Text.Json.Serialization.Metadata;
using System.Drawing.Drawing2D;
*/

public class fileComponent {
    private static JsonSerializerOptions thisJSOption = new JsonSerializerOptions {
        IncludeFields = true,
        WriteIndented = true
    };

    #region Resources_Json
    // parHalfPath is detailed path after Resources/Database
    public T importResourcesJson<T>(string parHalfPath, bool isTranslationRequired = true) where T : IDataInsurance {
        T tempResult;
        try {
            tempResult = JsonSerializer.Deserialize<T>(Resources.Load<TextAsset>(getResourcesPath(parHalfPath, isTranslationRequired)).ToString(), thisJSOption);
        } catch (Exception e) {
            Debug.Log("importResourcesJson failed to deserialize " + getResourcesPath(parHalfPath, isTranslationRequired) + " ((" + e);
            tempResult = default;
            tempResult.emergencyInit();
        }

        return tempResult;
    }

    public T[] importResourcesJsonArr<T>(string parPath, bool isTranslationRequired = true) where T : IDataInsurance {
        TextAsset[] tempTextAssetArr = Resources.LoadAll<TextAsset>(getResourcesPath(parPath, isTranslationRequired));
        T[] tempResult = new T[tempTextAssetArr.Length];
        for (int i = 0; i < tempResult.Length; i++) {
            try {
                tempResult[i] = JsonSerializer.Deserialize<T>(tempTextAssetArr[i].ToString(), thisJSOption);
            } catch (Exception e) {
                Debug.Log("importResourcesJsonArr failed to deserialize " + tempTextAssetArr[i].name + " from " + getResourcesPath(parPath, isTranslationRequired) + " ((" + e);
                tempResult[i] = default(T);
                tempResult[i].emergencyInit();
            }
        }

        return tempResult;
    }    
    #endregion Resources_Json

    #region Resources_ScriptabbleObject
    public T importResourcesSO<T>(string parHalfPath, bool isTranslationRequired = true) where T : ScriptableObject, IDataInsurance {
        T tempResult;
        try {
            tempResult = Resources.Load<T>(getResourcesPath(parHalfPath, isTranslationRequired));
        } catch (Exception e) {
            Debug.Log("fileComponent.getResourcesSO error : tried to import \"" + parHalfPath + "\"\n" + e.ToString());
            tempResult = ScriptableObject.CreateInstance<T>();
            tempResult.emergencyInit();
        }

        return tempResult;
    }
    #endregion Resources_ScriptabbleObject

    private string getResourcesPath(string parHalfPath, bool isTranslationRequired = true) {
        return "Database/" + (isTranslationRequired ? gameManager.GM.option.curLocalization.ToString() + "/" : "") + parHalfPath;
    }

    #region persistentDataPath
    // parPath should include file-extension
    public T importPersistentJson<T>(string parPath) where T : IDataInsurance {
        ensurePersistentPath(ref parPath);
        if (!File.Exists(parPath)) {
            Debug.Log("fileComponent.importPersistentJson results in error with path " + parPath);
            T tempResult = default;
            tempResult.emergencyInit();
            File.WriteAllText(parPath, JsonSerializer.Serialize(tempResult, tempResult.GetType(), thisJSOption));
            return tempResult;
        }

        return JsonSerializer.Deserialize<T>(File.ReadAllText(parPath), thisJSOption);
    }

    // exportJson exports only IDataInsurance object, only to Application.persistent
    public void exportPersistentJson(string parPath, IDataInsurance parData) {
        ensurePersistentPath(ref parPath);

        File.WriteAllText(parPath, JsonSerializer.Serialize(parData, parData.GetType(), thisJSOption));
    }

    // ensureSavePath ensures the argument-path to have the necessary directory & file-extension, and Save-directory to exist
    public static void ensurePersistentPath(ref string parPathPrimitive) {
        // Application.persistentDataPath    1
        IEnumerator<char> tempIEnumeratorPersistentPath = Application.persistentDataPath.GetEnumerator();
        IEnumerator<char> tempIEnumeratorParPath = parPathPrimitive.GetEnumerator();
        bool tempIsPersistentPathNeeded = false;
        while (tempIEnumeratorPersistentPath.MoveNext()) {
            if (!(tempIEnumeratorParPath.MoveNext() && tempIEnumeratorPersistentPath.Current == tempIEnumeratorParPath.Current)) {
                tempIsPersistentPathNeeded = true;
                break;
            }
        }

        // /Save/    1
        //bool tempIsSlashSaveNeeded = (parPathPrimitive.Length >= 5 && parPathPrimitive.Substring(0, 6) != "/Save/");
        if (parPathPrimitive.Length >= 5 && parPathPrimitive.Substring(tempIsPersistentPathNeeded ? 0 : Application.persistentDataPath.Length, 6) != "/Save/") {
            parPathPrimitive = "/Save/" + parPathPrimitive;
        }

        // Application.persistentDataPath 2
        if (tempIsPersistentPathNeeded) {
            parPathPrimitive = Application.persistentDataPath + parPathPrimitive;
        }

        /*
        // /Save/    2
        if (tempIsSlashSaveNeeded) {
            parPathPrimitive = "/Save/" + parPathPrimitive;
        }
        */

        // .json
        if (parPathPrimitive.Length >= 5 && parPathPrimitive.Substring(parPathPrimitive.Length - 5) != ".json") {
            parPathPrimitive = parPathPrimitive + ".json";
        }

        // if Save directory doesn't exist, create it
        if (!Directory.Exists(Application.persistentDataPath + "/Save")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/Save");
        }
    }
    #endregion persistentDataPath
}
