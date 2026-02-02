using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/*
    caution
    makerComponent creates dummy objects of all codableObjects whose codes to be compared when game starts
    making them is done only by Activator.createInstance and passing an array of int as arguement
    so concrete child class of codableObject should have int array as its parameter

    code explaination
            forth digit (count from the right lowest digit) represents case type, left three digits represents what the case truly is
            each forth digit represents each case type below
            0 : Thing (not used with codableObject)
            1 : circuit
            2 : skill
            3 : tool
            4 : effect
            5 : upgrade

            left three digits of caseBase identify what the case is, and it starts from 001 not 000

            third digit of circuit represents each circuit type below
            1 : sensor
            2 : navigator
            3 : selecter
            left two digits of circuit identify what the circuit is, and it starts from 01 not 00

            if code has fifth digit regardless of its value, the case is for test and expected not to be used in actual game

            lastly code is written in each creator of codableObject by programmer, so be cautious not to make a mistake
*/
public class codableObject : IParametable, ICloneable {
    private int code_ = -99;
    public int code {
        get {
            return code_;
        }
        protected set {
            if (code_ != -99) {
                return;
            }
            code_ = value;
        }
    }

    public mementoIParametable getMementoIParametable() {
        return new mementoIParametable(this, getParameters(), getReferences());
    }

    #region IParametable
    public virtual Dictionary<string, int[]> getParameters() {
        Dictionary<string, int[]> tempResult = new Dictionary<string, int[]>();

        // every last-leaf IParametable uses "concrete" as a key, adding the "concrete" key here blocks an error the last-leaf makes due to absense of key
        // other IParametables uses its class name as keys, and each restoreParameters methods of them contains preservation for absense of keys
        tempResult["concrete"] = new int[0];

        return tempResult;
    }

    public virtual List<object> getReferences() {
        return new List<object>();
    }

    // most codableObject can restore itself only with dicParameter, you should implement special codableObject's restore to use listReference
    public virtual void restore(mementoIParametable parmementoIParametable) {
        restoreParameters(parmementoIParametable.dicParameter);
        restoreReferences(parmementoIParametable.listReference);
    }

    public virtual void restoreParameters(IEnumerator<int> parParameters) { }

    public virtual void restoreParameters(Dictionary<string, int[]> parParameters) { }

    public virtual void restoreReferences(List<object> parRefernce) { }
    #endregion IParametable

    #region Clone
    public virtual object Clone() {
        codableObject tempResult = (codableObject)MemberwiseClone();
        tempResult.ClonePrepare();
        tempResult.restoreReferences(getReferences());
        tempResult.restoreParameters(getParameters());
        return tempResult;
    }

    // ClonePrepare only creates and prepares new object of each reference-type variable, true cloning is included in Clone()'s resotre methods
    protected virtual void ClonePrepare() { }
    #endregion Clone
}
