using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;

#region data_entities
[System.Serializable]
public struct dataWeapon {
    public string name;
    public int rangeMin;
    public int rangeMax;
    public int timerMax;
    public enumDamageType thisEnumDamageType;
    public enumAnimationType thisEnumAnimationType;
}

[System.Serializable]
public struct dataLevel {
    public string LevelName;
    public dataNotFriendlyThing[] EnemyWarriors;
    public dataNotFriendlyThing[] NeutralThings;
    public dataFriendlyThing[] FriendlyWarriors;
    public dataTool[] ToolsProvided;
}

[System.Serializable]
public struct dataNotFriendlyThing {
    public string NameThing;
    public int Coordinate0;
    public int Coordinate1;
    public int HP;
    public int[] SkillParameters;
    public dataTool[] ToolList;
    public int CodeSensorForMove; public int[] Parameter0;
    public int CodeNavigatorPrioritized; public int[] Parameter1;
    public int CodeNavigatorIdle; public int[] Parameter2;
    public int CodeSensorForSkill; public int[] Parameter3;
    public int CodeSelecterForSkill; public int[] Parameter4;
    public int CodeSelecterForAttack; public int[] Parameter5;
}

[System.Serializable]
public struct dataFriendlyThing {
    public string NameThing;
    public int Coordinate0;
    public int Coordinate1;
    public int HP;
    public int[] SkillParameters;
}

[System.Serializable]
public struct dataTool {
    public int CodeTool;
    public int[] ToolParameters;
}

[System.Serializable]
public struct dataArbitraryString {
    public string SwissArmyString;
}

[System.Serializable]
public struct dataArbitraryStringArray {
    public string[] SwissArmyStringArray;
}
#endregion data_entities

public class jsonComponent {

    public static string strLanguage = "English";

    public T getJson<T>(string parHalfPath, bool isTranslationRequired = true) where T : struct {
        string tempPath = "Assets/Resources/Database/" +
            (isTranslationRequired ? strLanguage + "/" : "") + 
            parHalfPath + ".json";

        T tempResult;
        if (File.Exists(tempPath)) {
            tempResult = JsonConvert.DeserializeObject<T>(File.ReadAllText(tempPath));
        } else {
            Debug.Log("json call failed, tried to call from \'" + tempPath + "\'");
            tempResult = new T();
        }
        return tempResult;
    }

}
