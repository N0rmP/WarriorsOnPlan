using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "dataBookThingName_default", menuName = "ScriptabbleObject/soBookThingName")]
public class soBookThingName : ScriptableObject, IDataInsurance {
    [System.Serializable]
    public struct pairCodeName {
        public int codeThing;
        public string nameThing;
    }

    [SerializeField]
    private pairCodeName[] arrPairCodeName;

    public void emergencyInit() {
        arrPairCodeName = new pairCodeName[0];
    }

    public string getThingName(int parThingCode) {
        return Array.Find(arrPairCodeName, (x) => x.codeThing == parThingCode).nameThing;
    }

    public void testBookWarriorName() {
        StringBuilder tempSB = new StringBuilder("dataBookWarriorName.testBookWarriorName\n");
        foreach (pairCodeName pcn in arrPairCodeName) {
            tempSB.Append(pcn.codeThing);
            tempSB.Append(" : ");
            tempSB.Append(pcn.nameThing);
            tempSB.Append("\n");
        }
        Debug.Log(tempSB.ToString());
    }
}