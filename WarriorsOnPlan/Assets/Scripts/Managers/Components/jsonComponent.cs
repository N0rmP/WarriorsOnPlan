using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System.Security.Policy;

#region data_entities
public interface IDataInsurance {
    public void emergencyInit();
}

[System.Serializable]
public struct dataWeapon : IDataInsurance {
    public string name;
    public int rangeMin;
    public int rangeMax;
    public int timerMax;
    public enumDamageType thisEnumDamageType;
    public enumAnimationType thisEnumAnimationType;

    public void emergencyInit() {
        name = "name error";
        rangeMin = 0;
        rangeMax = 1;
        timerMax = 0;
        thisEnumDamageType = enumDamageType.basic;
        thisEnumAnimationType = enumAnimationType.trigAttackPunch;
    }
}

[System.Serializable]
public struct dataLevel : IDataInsurance {
    public string LevelName;
    public dataNotFriendlyThing[] EnemyWarriors;
    public dataNotFriendlyThing[] NeutralThings;
    public dataFriendlyThing[] FriendlyWarriors;
    public dataTool[] ToolsProvided;

    public void emergencyInit() {
        LevelName = "level name error";
        EnemyWarriors = new dataNotFriendlyThing[0];
        NeutralThings = new dataNotFriendlyThing[0];
        FriendlyWarriors = new dataFriendlyThing[0];
        ToolsProvided = new dataTool[0];
    }
}

[System.Serializable]
public struct dataNotFriendlyThing : IDataInsurance {
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

    public void emergencyInit() {
        NameThing = "name error";
        Coordinate0 = 0;
        Coordinate1 = 0;
        HP = 1;
        SkillParameters = new int[0];
        ToolList = new dataTool[0];
        CodeSensorForMove = 0; Parameter0 = new int[0];
        CodeNavigatorPrioritized = 0; Parameter1 = new int[0];
        CodeNavigatorIdle = 0; Parameter2 = new int[0];
        CodeSelecterForSkill = 0; Parameter3 = new int[0];
        CodeSelecterForSkill = 0; Parameter4 = new int[0];
        CodeSelecterForAttack = 0; Parameter5 = new int[0];
    }
}

[System.Serializable]
public struct dataFriendlyThing : IDataInsurance {
    public string NameThing;
    public int Coordinate0;
    public int Coordinate1;
    public int HP;
    public int[] SkillParameters;

    public void emergencyInit() {
        NameThing = "name error";
        Coordinate0 = 0;
        Coordinate1 = 0;
        HP = 1;
        SkillParameters = new int[0];
    }
}

[System.Serializable]
public struct dataTool : IDataInsurance {
    public int CodeTool;
    public int[] ToolParameters;

    public void emergencyInit() {
        CodeTool = -1;
        ToolParameters = new int[2] { 2, 1 };
    }
}

[System.Serializable]
public struct dataArbitraryString : IDataInsurance{
    public string SwissArmyString;

    public void emergencyInit() {
        SwissArmyString = "string error";
    }
}

[System.Serializable]
public struct dataArbitraryStringArray : IDataInsurance {
    public string[] SwissArmyStringArray;

    public void emergencyInit() {
        SwissArmyStringArray = new string[10];
        Array.Fill(SwissArmyStringArray, "string arr error");
    }
}
#endregion data_entities

public class jsonComponent {

    public static string strLanguage = "English";

    public T getJson<T>(string parHalfPath, bool isTranslationRequired = true) where T : struct, IDataInsurance {
        string tempPath = "Assets/Resources/Database/" +
            (isTranslationRequired ? strLanguage + "/" : "") + 
            parHalfPath + ".json";

        T tempResult;
        if (File.Exists(tempPath)) {
            tempResult = JsonConvert.DeserializeObject<T>(File.ReadAllText(tempPath));
        } else {
            Debug.Log("json call failed, tried to call from \'" + tempPath + "\'");
            tempResult = new T();
            tempResult.emergencyInit();
        }
        return tempResult;
    }

}
