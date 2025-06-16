using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using Cases;
using Circuits;
using System.Linq;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;

/* 
        code explaination
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

public class makerComponent {
    // all codableObject instances in the Lists below are dummies whose code will be compared with the to-be-made codableObject
    private List<codableObject> listTotalSensors;
    private List<codableObject> listTotalNavigators;
    private List<codableObject> listTotalSelecters;

    private List<codableObject> listTotalSkills;
    private List<codableObject> listTotalTools;
    private List<codableObject> listTotalEffects;

    // setTotalDummies contains all codableObject whose code can't be identified such as codableObjects for test, whose code is written wrong, or plastic snack bag right in front of me I forgot to throw away etc.
    private List<codableObject> listTotalDummies;

    // ★ 만약 makerComponent의 생성이 너무 오래 걸린다면 생성자 내에서 Coroutine으로 병렬처리 돌려버릴 것
    public makerComponent() {
        listTotalSensors = new List<codableObject>();
        listTotalNavigators = new List<codableObject>();
        listTotalSelecters = new List<codableObject>();
        listTotalSkills = new List<codableObject>();
        listTotalTools = new List<codableObject>();
        listTotalEffects = new List<codableObject>();
        listTotalDummies = new List<codableObject>();

        // create total dummy-instances of each codableObject
        int[] tempDummyParameter = new int[10] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        codableObject tempCodableObject;

        // addIEnumerable take IEnumerable<Type> as argument and ignores non-codableObject indice in it
        // addIEnumerable creates one dummy instance of each codableObject-Types and classify them into the Lists above
        void addIEnumerable(IEnumerable<Type> parContainer) {
            foreach (Type t in parContainer) {
                if (t.IsAbstract || t == typeof(circuitHub)) {
                    continue;
                }

                tempCodableObject = Activator.CreateInstance(t, tempDummyParameter) as codableObject;

                if (tempCodableObject == null) {
                    continue;
                }

                getAdequateSet(tempCodableObject.code).Add(tempCodableObject);
            }
        }

        // rake all codableObject
        addIEnumerable(
            typeof(caseBase).Assembly.GetTypes().Where(
                (x) => x.IsSubclassOf(typeof(codableObject))
            )
        );

        comparerCode tempComparerCode = new comparerCode();
        listTotalSensors.Sort(tempComparerCode);
        listTotalNavigators.Sort(tempComparerCode);
        listTotalSelecters.Sort(tempComparerCode);
        listTotalSkills.Sort(tempComparerCode);
        listTotalTools.Sort(tempComparerCode);
        listTotalEffects.Sort(tempComparerCode);
        listTotalDummies.Sort(tempComparerCode);
    }

    private List<codableObject> getAdequateSet(int parCode) {
        return (parCode / 1000) switch {
            1 => // circuit
                (parCode / 100 - 10) switch {
                    1 => listTotalSensors,
                    2 => listTotalNavigators,
                    3 => listTotalSelecters,
                    _ => listTotalDummies
                },
            // anyway caseBase
            2 => listTotalSkills,
            3 => listTotalTools,
            4 => listTotalEffects,
            _ => listTotalDummies
        };
    }

    /*
        iterateAdequateSet uses parCode only for checking category of codableObject
        it means you can only pass forth digit (+third digit for circuitAbst) with zero in other digits
    */
    public IEnumerable iterateAdequateSet(int parCode) {
        foreach (codableObject co in getAdequateSet(parCode)) {
            yield return co;
        }
    }

    private codableObject getAdequateCodableObject(int parCode) {
        // ★ 여유날 때 이거 이진탐색으로 바꿔볼 것... 근데 시간 없지 않을까
        foreach (codableObject co in getAdequateSet(parCode)) {
            if (co.code == parCode) {
                return co;
            }
        }

        Debug.Log("makerComponent.getAdequateCodableObject error : tried to get code " + parCode);
        return null;
    }


    #region MAKE
    // pp is parParameters
    public T makeCodableObject<T>(int parCode, IEnumerable<int> pp) where T : codableObject {
        T tempResult = getAdequateCodableObject(parCode)?.Clone() as T;
        tempResult?.restoreParameters(pp.GetEnumerator());

        if (tempResult == null) {
            Debug.Log("makerComponent.makeCodableObject<T> returns null for code " + parCode + " / T was " + typeof(T));
        }

        return tempResult;
    }

    public codableObject makeCodableObject(int parCode, IEnumerable<int> pp) {
        return makeCodableObject<codableObject>(parCode, pp);
    }
    #endregion MAKE

    // it's MAKER but... when you need to get just a single-description to show on UI, then sneak carefully and calmly
    public IInfo sneakISingleInfo(int parCode) {
        codableObject tempResult = getAdequateCodableObject(parCode);
        if (tempResult is IInfo) {
            return (tempResult as IInfo);
        } else {
            Debug.Log("makerComponent.sneakISingleInfo error : tried to sneak code " + parCode);
            return listTotalSensors.Single() as IInfo; // sensorAbst implements ISingleInfo and this line can't make a error
        }
    }

    private class comparerCode : IComparer<codableObject> {
        public int Compare(codableObject x, codableObject y) {
            return x.code - y.code;
        }
    }
}
