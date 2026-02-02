using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public interface IDataInsurance {
    public void emergencyInit();
}

#region InGame
public class dataLevel : IDataInsurance {
    public float[] MapPosition; // MapPosition represents the position of button on the map with anchors
    public int LevelCode;
    public bool IsBossLevel;
    public int[] NextLevelCode;
    public string Differentiater;
    public string Placabler;
    public int[] PlacablerParameter;
    public dataNotFriendlyThing[] EnemyWarriors;
    public dataNotFriendlyThing[] NeutralThings;
    public dataFriendlyThing[] FriendlyWarriors;
    public dataIParametable[] ToolsProvided;

    public void emergencyInit() {
        MapPosition = new float[2] { 0.1f, 0.1f };
        LevelCode = 90101;
        IsBossLevel = false;
        Differentiater = "Tutorial00";
        Placabler = "RowCol";
        PlacablerParameter = new int[4] { 0, 6, 0, 2 };
        EnemyWarriors = new dataNotFriendlyThing[0];
        NeutralThings = new dataNotFriendlyThing[0];
        FriendlyWarriors = new dataFriendlyThing[0];
        ToolsProvided = new dataIParametable[0];
    }
}

public class dataNotFriendlyThing : IDataInsurance {
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

public class dataFriendlyThing : IDataInsurance {
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

public class dataIParametable : IDataInsurance {
    public int CodeIParametable;
    [JsonInclude]
    private int[] Parameters { get; set; }

    public IEnumerator<int> getParametersEnumerator() {
        return Parameters.GetEnumerator<int>();

    }

    public void emergencyInit() {
        CodeIParametable = 92001;
        Parameters = new int[2] { 2, 1 };
    }
}
#endregion InGame

#region book
// ★ 보류, 추후 전투 완료 화면에 무엇이 추가될지 모르기 때문에 이걸 차라리 dataBookWords에 이관하는 게 나을 수 있음
// [CreateAssetMenu(fileName = "dataBookCombatResult_default", menuName = "ScriptabbleObject/dataBookCombatResult", order = 5)]
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
public class dataSaveBasicMap : IDataInsurance {
    [JsonInclude]
    private List<int> upgradeDone;
    [JsonInclude]
    private List<int> levelCleared;
    public int stars { get; private set; }

    public dataSaveBasicMap() {
        emergencyInit();
    }

    public dataSaveBasicMap(List<int> parUpgrades, List<int> parLevelsCleared, int parStars) {
        upgradeDone = parUpgrades;
        levelCleared = parLevelsCleared;
        stars = parStars;
    }

    #region field_control
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

    public void addUpgradeDoneRange(IEnumerable<int> parBulCodeCol) {
        foreach (int i in parBulCodeCol) {
            addUpgradeDone(i);
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
    
    public int[] getLevelCleared() {
        return levelCleared.ToArray();
    }

    public int[] getUpgradeDone() {
        return upgradeDone.ToArray();
    }

    public void addStars(int parValue) {
        stars += Math.Max(0, parValue);
    }

    public void setStars(int parValue) { 
        stars = Math.Max(0, parValue);
    }
    #endregion field_control

    public void ensureSaveBasicValid() {
        if (upgradeDone == null || levelCleared == null) {
            emergencyInit();
        }
    }

    public void emergencyInit() {
        upgradeDone = new List<int>();
        levelCleared = new List<int>();
        stars = 0;
    }

    public void testDataSaveBasicMap() {
        StringBuilder tempSB = new StringBuilder("testDataSaveBasicMap\nupgradeDone : ");
        tempSB.Append(upgradeDone);
        tempSB.Append("\nlevelCleared : ");
        tempSB.Append(levelCleared);
        tempSB.Append("\nstars : ");
        tempSB.Append(stars);
        Debug.Log(tempSB.ToString());
    }
}

public struct dataSaveLevel : IDataInsurance {
    public int LevelCode;
    public bool IsClear;
    // ★ 이거 나중에 플레이어 분석-동적 생성이랑 연결시키려면 뭐 저장시켜야 할지 살펴보기

    public void emergencyInit() {
        LevelCode = 90101;
        IsClear = false;
    }
}
#endregion save

#region upgrade

// ★ 업그레이드 트리 개선과 함께 SO화 진행, 자유도를 높일 수 있도록
// each array of tree contains only the root nodes, leaves will be contained in each leaf
public struct dataUpgradeTree : IDataInsurance {
    public dataUpgradeLeaf[] ArrUpgradeTreeZero { get; set; }
    public dataUpgradeLeaf[] ArrUpgradeTreeOne;
    public dataUpgradeLeaf[] ArrUpgradeTreeTwo;
    public dataUpgradeTreeEdge[] ArrUpgradeTreeEdgeZero;
    public dataUpgradeTreeEdge[] ArrUpgradeTreeEdgeOne;
    public dataUpgradeTreeEdge[] ArrUpgradeTreeEdgeTwo;

    public void emergencyInit() {
        ArrUpgradeTreeZero = new dataUpgradeLeaf[0];
        ArrUpgradeTreeOne = new dataUpgradeLeaf[0];
        ArrUpgradeTreeTwo = new dataUpgradeLeaf[0];
        ArrUpgradeTreeEdgeZero = new dataUpgradeTreeEdge[0];
        ArrUpgradeTreeEdgeOne = new dataUpgradeTreeEdge[0];
        ArrUpgradeTreeEdgeTwo = new dataUpgradeTreeEdge[0];
    }
}
public record dataUpgradeTreeEdge {
    public int parent;
    public int child;
}

public struct dataUpgradeLeaf : IDataInsurance {
    public int LeafCode;
    public int UpgradeCode;
    public int[] Parameters;

    public void emergencyInit() {
        LeafCode = 1109;
        UpgradeCode = 5001;
        Parameters = new int[7] { 1, 0, 0, 0, 1, 3, 1 };
    }
}
#endregion upgrade

public struct dataOption : IDataInsurance {
    public float MasterVolume;
    public float BgmVolume;
    public float SeVolume;
    public FullScreenMode ScreenMode;
    public int ResolutionIndex;
    public int Localization;

    public dataOption(float parMasterVolume, float parBgmVolume, float parSeVolume, FullScreenMode parScreenMode, int parResolutionIndex, int parLocalization) {
        MasterVolume = parMasterVolume;
        BgmVolume = parBgmVolume;
        SeVolume = parSeVolume;
        ScreenMode = parScreenMode;
        ResolutionIndex = parResolutionIndex;
        Localization = parLocalization;
    }

    public void emergencyInit() {
        MasterVolume = 0.7f;
        BgmVolume = 0.7f;
        SeVolume = 0.7f;
        ScreenMode = FullScreenMode.ExclusiveFullScreen;
        ResolutionIndex = Screen.resolutions.Length - 1;
        Localization = 0;
    }
    public void testDataOption() {
        StringBuilder tempSB = new StringBuilder("testDataOption\nMasterVolume : ");
        tempSB.Append(MasterVolume);
        tempSB.Append("\nBgmVolume : ");
        tempSB.Append(BgmVolume);
        tempSB.Append("\nSeVolume : ");
        tempSB.Append(SeVolume);
        tempSB.Append("\nScreenMode : ");
        tempSB.Append(ScreenMode);
        tempSB.Append("\nResolutionIndex : ");
        tempSB.Append(ResolutionIndex);
        tempSB.Append("\nLocalization : ");
        tempSB.Append(Localization);
        Debug.Log(tempSB.ToString());
    }
}

#region test
public struct dataEnumTest : IDataInsurance {
    public enumSide test { get; set; }

    public void emergencyInit() {
        test = enumSide.none;
    }
}
#endregion test