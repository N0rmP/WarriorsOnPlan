using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum enumMapType {
    None = -9,
    Normal = 1,
    Hard = 2,
    Elite = 3,
    Test = 9,
}

public class mapManager : MonoBehaviour {
    public static mapManager MM = null;

    public mapUIComponent MUC { get; private set; }
    public upgradeComponent UC { get; private set; }

    private IMapper curMapper;
    private Dictionary<enumMapType, IMapper> dictMapper;

    public void Awake() {
        if (MM == null) {
            MM = this;
        } else {
            Destroy(this);
        }
        
        MUC = new mapUIComponent();
        UC = new upgradeComponent();

        dictMapper = new Dictionary<enumMapType, IMapper>();
        mapperBasic tempMapperBasic = new mapperBasic();
        dictMapper.Add(enumMapType.Normal, tempMapperBasic);
        dictMapper.Add(enumMapType.Hard, tempMapperBasic);
        dictMapper.Add(enumMapType.Elite, tempMapperBasic);
    }

    #region relay
    public void prepareMap() {
        curMapper = dictMapper[gameManager.GM.curMapType];
        curMapper.prepareMap();
    }

    public void doWhenCombatVictory() {
        curMapper.doWhenCombatVictory();
    }

    public void doWhenCombatDefeated() {
        curMapper.doWhenCombatDefeated();
    }

    // load might be included in prepareMap
    public void save() {
        curMapper.save();
    }
    #endregion relay
}