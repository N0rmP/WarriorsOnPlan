using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using Cases;
using Circuits;
using System.Linq;
using TMPro;

public class makerComponent {
    // all codableObject instances in the HashSets below are dummies whose code will be compared with the to-be-made codableObject
    private HashSet<codableObject> setTotalSensors;
    private HashSet<codableObject> setTotalNavigators;
    private HashSet<codableObject> setTotalSelecters;

    private HashSet<codableObject> setTotalSkills;
    private HashSet<codableObject> setTotalTools;
    private HashSet<codableObject> setTotalEffects;

    // setTotalDummies contains all codableObject whose code can't be identified such as codableObjects for test, whose code is written wrong, or plastic snack bag right in front of me I forgot to throw away etc.
    private HashSet<codableObject> setTotalDummies;

    // ★ 만약 makerComponent의 생성이 너무 오래 걸린다면 생성자 내에서 Coroutine으로 병렬처리 돌려버릴 것
    public makerComponent() {
        setTotalSensors = new HashSet<codableObject>();
        setTotalNavigators = new HashSet<codableObject>();
        setTotalSelecters = new HashSet<codableObject>();
        setTotalSkills = new HashSet<codableObject>();
        setTotalTools = new HashSet<codableObject>();
        setTotalEffects = new HashSet<codableObject>();
        setTotalDummies = new HashSet<codableObject>();

        // create total dummy-instances of each codableObject
        int[] tempDummyParameter = new int[10] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        codableObject tempCodableObject;

        // addIEnumerable take IEnumerable<Type> as argument and ignores non-codableObject indice in it
        // addIEnumerable creates one dummy instance of each codableObject-Types and classify them into the HashSets above
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
    }

    private HashSet<codableObject> getAdequateSet(int parCode) {
        return (parCode / 1000) switch {
            1 => // circuit
                (parCode / 100 - 10) switch {
                    1 => setTotalSensors,
                    2 => setTotalNavigators,
                    3 => setTotalSelecters,
                    _ => setTotalDummies
                },
            // anyway caseBase
            2 => setTotalSkills,
            3 => setTotalTools,
            4 => setTotalEffects,
            _ => setTotalDummies
        };
    }

    public IEnumerable iterateAdequateSet(int parCode) {
        foreach (codableObject co in getAdequateSet(parCode)) {
            yield return co;
        }
    }

    private codableObject getAdequateCodableObject(int parCode) {
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
        return tempResult;
    }

    public codableObject makeCodableObject(int parCode, IEnumerable<int> pp) {
        return makeCodableObject<codableObject>(parCode, pp);
    }
    #endregion MAKE

    // it's MAKER but... when you need to get just a single-description to show on UI, then sneak carefully and calmly
    public ISingleInfo sneakISingleInfo(int parCode) {
        codableObject tempResult = getAdequateCodableObject(parCode);
        if (tempResult is ISingleInfo) {
            return (tempResult as ISingleInfo);
        } else {
            Debug.Log("makerComponent.sneakISingleInfo error : tried to sneak code " + parCode);
            return setTotalSensors.Single() as ISingleInfo; // sensorAbst implements ISingleInfo and this line can't make a error
        }
    }
}
