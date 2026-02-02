using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Differentiaters;
using System.Linq;

public class differentiaterComponent : MonoBehaviour {
    private differentiaterBase curDifferentiater = null;

    private differentiaterBase[] arrDifferentiaterStorage;
    private differentiaterNormalBasic differentiaterNormalDefault;
    // private differentiaterHardBasic differentiaterHardDefault;
    // private differentiaterEliteBasic differentiaterEliteDefault;

    public void Awake() {
        IEnumerable<Type> tempArrType = typeof(differentiaterBase).Assembly.GetTypes().Where(
            (x) => x.IsSubclassOf(typeof(differentiaterBase)) && x.Name != "differentiaterBase"
        );
        List<differentiaterBase> tempListStorage = new List<differentiaterBase>();
        foreach (Type typ in tempArrType) {
            tempListStorage.Add((differentiaterBase)Activator.CreateInstance(typ));
            switch (tempListStorage.Last()) {
                case differentiaterNormalBasic tempDNB:
                    differentiaterNormalDefault = tempDNB;
                    break;
                    /*
                    case differentiaterHardBasic tempDHB:
                        differentiaterHardDefault = tempDHB;
                        break;
                    case differentiaterEliteBasic tempDEB:
                        differentiaterEliteDefault = tempDEB;
                        break;
                        */
            }
        }
        arrDifferentiaterStorage = tempListStorage.ToArray();
    }

    public void Update() {
        curDifferentiater?.checkAndDo();
    }    

    public void setDifferentiater(string parDifferentiaterName) {
        differentiaterBase getAdequateDifferentiaterDefault() {
            return gameManager.GM.curMapType switch {
                enumMapType.Normal => differentiaterNormalDefault,
                enumMapType.Hard => null,       // differentiaterHardDefault,
                enumMapType.Elite => null,      // differentiaterEliteDefault,
                _ => null
            };
        }

        if (parDifferentiaterName == null || parDifferentiaterName.Length == 0) {
            curDifferentiater = getAdequateDifferentiaterDefault();
            return;
        }

        foreach (differentiaterBase db in arrDifferentiaterStorage) {
            if (db.GetType().Name.Contains(parDifferentiaterName)) {
                curDifferentiater = db;
                return;
            }
        }
        curDifferentiater = getAdequateDifferentiaterDefault();
    }

    public void initDifferentiater() {
        curDifferentiater?.init();
    }

    public void makeDifferentiaterLeave() {
        curDifferentiater?.restoreWhenLeave();
        curDifferentiater = null;
    }
}
