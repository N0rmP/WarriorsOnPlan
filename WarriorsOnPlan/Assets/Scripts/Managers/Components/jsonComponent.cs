using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;

#region data_entities
[System.Serializable]
public class dataWeapon {
    public string name;
    public int rangeMin;
    public int rangeMax;
    public int timerMax;
    public enumDamageType thisEnumDamageType;
    public enumAnimationType thisEnumAnimationType;
}

[System.Serializable]
public class dataLevel {
    public string LevelName;
    public dataNotFriendlyThing[] EnemyWarriors;
    public dataNotFriendlyThing[] NeutralThings;
    public dataFriendlyThing[] FriendlyWarriors;
    public dataTool[] ToolsProvided;
}

[System.Serializable]
public class dataNotFriendlyThing {
    public string NameThing;
    public int Coordinate0;
    public int Coordinate1;
    public int HP;
    public int[] SkillParameters;
    public dataTool[] ToolList;
    public int CodeSelecterForAttack; public int[] Parameter0;
    public int CodeSelecterForSkill; public int[] Parameter1;
    public int CodeMoveSensorIdle; public int[] Parameter2;
    public int CodeMoveSensorPrioritized; public int[] Parameter3;
    public int CodeNavigatorIdle; public int[] Parameter4;
    public int CodeNavigatorPrioritized; public int[] Parameter5;
    public int CodeSkillSensorIdle; public int[] Parameter6;
    public int CodeSkillSensorPrioritized; public int[] Parameter7;
}

[System.Serializable]
public class dataFriendlyThing {
    public string NameThing;
    public int Coordinate0;
    public int Coordinate1;
    public int HP;
    public int[] SkillParameters;
}

[System.Serializable]
public class dataTool {
    public int CodeTool;
    public int[] ToolParameters;
}
#endregion data_entities

public class jsonComponent {
    public T getJson<T>(string parPath) where T : class {
        if (File.Exists(parPath)) {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(parPath));
        } else {
            Debug.Log("json call failed");
            return null;
        }
    }

}
