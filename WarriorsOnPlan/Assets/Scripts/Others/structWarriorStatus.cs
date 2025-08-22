using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// it can't be passed if it's struct... ffffffffff
public class structWarriorStatus {
    public int weaponAmplifierAdd;
    private int weaponAmplifierMultiply_;
    public int weaponAmplifierMultiply {
        get {
            return weaponAmplifierMultiply_;
        }
        set {
            weaponAmplifierMultiply_ = Math.Max(0, value);
        }
    }

    public int skillAmplifierAdd;
    private int skillAmplifierMultiply_;
    public int skillAmplifierMultiply {
        get {
            return skillAmplifierMultiply_;
        }
        set {
            skillAmplifierMultiply_ = Math.Max(0, value);
        }
    }

    public int armorAdd;
    private int armorMultiply_;
    public int armorMultiply {
        get {
            return armorMultiply_;
        }
        set {
            armorMultiply_ = Math.Max(0, value);
        }
    }

    public structWarriorStatus(int parDummy) {
        weaponAmplifierAdd = 0;
        weaponAmplifierMultiply_ = 100;
        skillAmplifierAdd = 0;
        skillAmplifierMultiply_ = 100;
        armorAdd = 0;
        armorMultiply_ = 100;
    }

    public void reset() {
        weaponAmplifierAdd = 0;
        weaponAmplifierMultiply_ = 100;
        skillAmplifierAdd = 0;
        skillAmplifierMultiply_ = 100;
        armorAdd = 0;
        armorMultiply_ = 100;
    }
}
