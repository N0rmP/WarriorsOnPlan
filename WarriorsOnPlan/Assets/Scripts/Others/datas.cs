using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataInsurance {
    public void emergencyInit();
}
public struct dataLevel : IDataInsurance {
    public float[] MapPosition; // MapPosition represents the position of button on the map with anchors
    public int LevelCode;
    public bool IsBossLevel;
    public int[] NextLevelCode;
    public dataNotFriendlyThing[] EnemyWarriors;
    public dataNotFriendlyThing[] NeutralThings;
    public dataFriendlyThing[] FriendlyWarriors;
    public dataIParametable[] ToolsProvided;

    public void emergencyInit() {
        MapPosition = new float[2] { 0.1f, 0.1f };
        LevelCode = 90101;
        IsBossLevel = false;
        EnemyWarriors = new dataNotFriendlyThing[0];
        NeutralThings = new dataNotFriendlyThing[0];
        FriendlyWarriors = new dataFriendlyThing[0];
        ToolsProvided = new dataIParametable[0];
    }
}

public struct dataWeapon : IDataInsurance {
    public string name;
    public int rangeMin;
    public int rangeMax;
    public int timerMax;
    public enumDamageType thisEnumDamageType;
    public enumAttackAnimation thisEnumAnimationType;

    public void emergencyInit() {
        name = "dataWeapon name error";
        rangeMin = 0;
        rangeMax = 1;
        timerMax = 0;
        thisEnumDamageType = enumDamageType.basic;
        thisEnumAnimationType = enumAttackAnimation.trigAttackPunch;
    }
}

#region dataThing
public struct dataNotFriendlyThing : IDataInsurance {
    public string NameThing;
    public int Coordinate0;
    public int Coordinate1;
    public int HP;
    public int[] SkillParameters;
    public dataIParametable[] ToolList;
    public int CodeNavigatorIdle; public int[] Parameter2;
    public int CodeSensorForMove; public int[] Parameter0;
    public int CodeNavigatorPrioritized; public int[] Parameter1;
    public int CodeSensorForSkill; public int[] Parameter3;
    public int CodeSelecterForSkill; public int[] Parameter4;
    public int CodeSelecterForAttack; public int[] Parameter5;

    public void emergencyInit() {
        NameThing = "dataNotFriendlyThing name error";
        Coordinate0 = 0;
        Coordinate1 = 0;
        HP = 1;
        SkillParameters = new int[0];
        ToolList = new dataIParametable[0];
        CodeNavigatorIdle = 0; Parameter2 = new int[0];
        CodeSensorForMove = 0; Parameter0 = new int[0];
        CodeNavigatorPrioritized = 0; Parameter1 = new int[0];
        CodeSensorForSkill = 0; Parameter3 = new int[0];
        CodeSelecterForSkill = 0; Parameter4 = new int[0];
        CodeSelecterForAttack = 0; Parameter5 = new int[0];
    }
}

public struct dataFriendlyThing : IDataInsurance {
    public string NameThing;
    public int Coordinate0;
    public int Coordinate1;
    public int HP;
    public int[] SkillParameters;

    public void emergencyInit() {
        NameThing = "dataFriendlyThing name error";
        Coordinate0 = 0;
        Coordinate1 = 0;
        HP = 1;
        SkillParameters = new int[0];
    }
}
#endregion dataThing

public struct dataIParametable : IDataInsurance {
    public int CodeIParametable;
    public int[] Parameters;

    public void emergencyInit() {
        CodeIParametable = 92001;
        Parameters = new int[2] { 2, 1 };
    }
}

#region dataArbitrary
public struct dataArbitraryString : IDataInsurance {
    public string SwissArmyString;

    public void emergencyInit() {
        SwissArmyString = "dataArbitraryString error";
    }
}

public struct dataArbitraryStringArray : IDataInsurance {
    public string[] SwissArmyStringArray;

    public void emergencyInit() {
        SwissArmyStringArray = new string[10];
        Array.Fill(SwissArmyStringArray, "dataArbitraryStringArray error");
    }
}
#endregion dataArbitrary

#region book
public struct dataBookWords : IDataInsurance {
    public string strMelee;
    public string strNumber;
    public string strReady;
    public string strVictory;
    public string strDefeated;

    public string strTool;
    public string strEffect;
    public string strSkill;
    public string strUpgrade;

    public string strInterfere;
    public string strAction;
    public string strAdd;
    public string strAttack;
    public string strDamaged;
    public string strDealDamage;
    public string strDeath;
    public string strForcedMove;
    public string strHpDecrease;
    public string strHpIncrease;
    public string strMove;
    public string strFocussing;

    public void emergencyInit() {
        strMelee = "Melee";
        strNumber = "(Number)";
        strReady = "Ready";
        strVictory = "Victory";
        strDefeated = "Defeated";

        strTool = "Tool";
        strEffect = "Effect";
        strSkill = "Skill";
        strUpgrade = "Upgrade";

        strInterfere = "Denied";
        strAction = "Action";
        strAdd = "Adding";
        strAttack = "Attack";
        strDamaged = "Taking Damage";
        strDealDamage = "Dealing Damage";
        strDeath = "Death";
        strForcedMove = "Forced Move";
        strHpDecrease = "Hp Decrease";
        strHpIncrease = "Hp Increase";
        strMove = "Move";
        strFocussing = "Focussing";
    }
}

public struct dataBookConfirmQuestion : IDataInsurance {
    public string strQuestionResetInitial;

    public void emergencyInit() {
        strQuestionResetInitial = "All Preparation Including Tools, Circuits, Warriors' Positions Returns to the Initial State.";
    }
}

public struct dataBookPopupAlert : IDataInsurance {
    public string strAlertNoAttackTarget;
    public string strAlertNoSkillTarget;

    public void emergencyInit() {
        strAlertNoAttackTarget = "No Attack Target";
        strAlertNoSkillTarget = "No Skill Target";
    }
}

public struct dataBookCombatResult : IDataInsurance {
    public string strActionElapsed;
    public string strTotalDamageDealt;
    public string strTotalDamageTaken;

    public void emergencyInit() {
        strActionElapsed = "Actions Elapsed";
        strTotalDamageDealt = "Total Dealt Damage";
        strTotalDamageTaken = "Total Taken Damage";
    }
}
#endregion book

#region save
public struct dataSaveBasicMap : IDataInsurance {
    [JsonProperty]
    private List<int> upgradeDone;
    [JsonProperty]
    private List<int> levelCleared;

    public dataSaveBasicMap(List<int> parUpgrades, List<int> parLevelsCleared) {
        upgradeDone = parUpgrades;
        levelCleared = parLevelsCleared;
    }

    #region management
    public void addLevelCleared(int parLevelCode) {
        if (!levelCleared.Contains(parLevelCode)) {
            levelCleared.Add(parLevelCode);
        }
    }

    public void addUpgradeDone(int parBulCode) {
        if (!upgradeDone.Contains(parBulCode)) {
            upgradeDone.Add(parBulCode);
        }
    }

    public void removeUpgradeDone(int parBulCode) {
        upgradeDone.Remove(parBulCode);
    }

    public void clearLevelCleared() {
        levelCleared.Clear();
    }

    public void clearUpgradeDone() {
        upgradeDone.Clear();
    }
    #endregion management

    public int[] getLevelCleared() {
        return levelCleared.ToArray();
    }

    public int[] getUpgradeDone() {
        return upgradeDone.ToArray();
    }

    public void emergencyInit() {
        upgradeDone = new List<int>();
        levelCleared = new List<int>();
    }
}

public struct dataSaveLevel : IDataInsurance {
    public int LevelCode;
    public bool IsClear;
    // ★ 이거 나중에 동적 생성이랑 연결시키려면 뭐 저장시켜야 할지 살펴보기

    public void emergencyInit() {
        LevelCode = 90101;
        IsClear = false;
    }
}
#endregion save

// each array of tree contains only the root nodes, leaves will be contained in each leaf
public struct dataUpgradeTree : IDataInsurance {
    public dataUpgradeLeaf[] ArrUpgradeTreeZero;
    public dataUpgradeLeaf[] ArrUpgradeTreeOne;
    public dataUpgradeLeaf[] ArrUpgradeTreeTwo;

    public void emergencyInit() {
        ArrUpgradeTreeZero = new dataUpgradeLeaf[0];
        ArrUpgradeTreeOne = new dataUpgradeLeaf[0];
        ArrUpgradeTreeTwo = new dataUpgradeLeaf[0];
    }
}

public struct dataUpgradeLeaf : IDataInsurance {
    public int code;
    public int[] parameters;
    public dataUpgradeLeaf[] next;

    public void emergencyInit() {
        code = 5101;
        parameters = new int[0];
        next = new dataUpgradeLeaf[0];
    }
}

#region test
public struct dataEnumTest : IDataInsurance {
    [JsonConverter(typeof(StringEnumConverter))]
    public enumSide test { get; set; }

    public void emergencyInit() {
        test = enumSide.none;
    }
}
#endregion test