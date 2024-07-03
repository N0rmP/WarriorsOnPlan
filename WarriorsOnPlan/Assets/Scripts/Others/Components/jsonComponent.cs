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
        public List<List<System.Object>> EnemyWarriors;
        public List<List<System.Object>> NeutralThings;
        public List<List<System.Object>> FriendlyWarriors;
        public List<List<System.Object>> ToolsProvided;
    }
    #endregion data_entities
}
