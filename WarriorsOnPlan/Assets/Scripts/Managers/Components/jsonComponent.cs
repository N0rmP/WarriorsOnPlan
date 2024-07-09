using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jsonComponent {

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
        public List<dataNotFriendlyThing> EnemyWarriors;
        public List<dataNotFriendlyThing> NeutralThings;
        public List<dataFriendlyThing> FriendlyWarriors;
        public List<dataTool> ToolsProvided;
    }

    [System.Serializable]
    public class dataNotFriendlyThing {
        public int CodeThing;
        public int Coordinate0;
        public int Coordinate1;
        public int HP;
        public List<int> SkillParameters;
        public dataTool[] ToolList;
        public int CodeSelecterForAttack;       public int[] Parameter0;
        public int CodeSelecterForSkill;        public int[] Parameter1;
        public int CodeMoveSensorPrioritized;   public int[] Parameter2;
        public int CodeNavigatorIdle;           public int[] Parameter3;
        public int CodeNavigatorPrioritized;    public int[] Parameter4;
        public int CodeSkillSensorIdle;         public int[] Parameter5;
        public int CodeSkillSensorPrioritized;  public int[] Parameter6;
    }

    [System.Serializable]
    public class dataFriendlyThing {
        public int CodeThing;
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

}
