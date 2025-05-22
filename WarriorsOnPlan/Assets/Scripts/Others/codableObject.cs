using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/*
    caution
    makerComponent creates all codableObjects whose codes to be compared when game starts
    and regardless of their type making them is done only by Activator.createInstance and passing an array of int as arguement
    so concrete child class of codableObject should have int array as its parameter
*/
public class codableObject : IParametable, ICloneable {
    /* 
        caseBase code explaination
            forth digit (count from the right lowest digit) represents case type, left three digits represents what the case truly is
            each forth digit represents each case type below
            1 : circuit
            2 : skill
            3 : tool
            4 : effect

            left three digits of caseBase identify what the case is, and it starts from 001 not 000

            third digit of circuit represents each circuit type below
            1 : sensor
            2 : navigator
            3 : selecter
            left two digits of circuit identify what the circuit is, and it starts from 001 not 000

            if code has fifth digit regardless of its value, the case is for test and expected not to be used in actual game

            lastly code is written in each creator of codableObject by programmer, so be cautious not to make a mistake
    */
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

    public codableObject(IEnumerable<int> parArrParameter) {
        restoreParameters(parArrParameter.GetEnumerator());
    }

    public mementoIParametable getMementoIParametable() {
        return new mementoIParametable(this, getParameters(), getReference());
    }

    public virtual Dictionary<string, int[]> getParameters() {
        Dictionary<string, int[]> tempResult = new Dictionary<string, int[]>();

        // every last-leaf IParametable uses "concrete" as a key, adding the "concrete" key here blocks an error the last-leaf makes due to absense of key
        // other IParametables uses its class name as keys, and each restoreParameters methods of them contains preservation for absense of keys
        tempResult["concrete"] = null;

        return tempResult;
    }

    public virtual List<object> getReference() {
        return new List<object>();
    }

    public virtual void restore(mementoIParametable parmementoIParametable) {
        restoreParameters(parmementoIParametable.dicParameter);
    }

    public virtual void restoreParameters(IEnumerator<int> parParameters) {
        parParameters.Reset();
    }

    public virtual void restoreParameters(Dictionary<string, int[]> parParameters) { }

    public virtual object Clone() {
        return MemberwiseClone();
    }

    /*
        �� ���� deepCopy ���� �޼���� �ʿ��������� ����������, ���� �ʿ������ٸ� deepcopy ���� �޼��带 �����ϰ� Clone �޼��带 ������ ��
        �ٸ� ���� �����ϱ�� codableObject�� ��ü�� �ʵ�� �����ٸ� �۵��� �ʿ��� ������Ʈ�� �ƴ϶� ������ Ư�� warrior�� �����ϴ� ���̹Ƿ� ���� ���簡 ���ʿ���
        �Ʒ��� ����Ǵ� deepcopy�� �ʿ������� ����
        1. caseBase�� Sprite�� ������ �� ���� ��
        2. circuitAbst �迭���� Ư�� warrior�� �����ϴ� ���� MemberwiseClone�� �������� �� �� ��

    protected virtual void deepCopyTo() { }
    public virtual void deepCopyFrom() { }
    */
}
