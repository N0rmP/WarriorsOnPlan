using Cases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

// ★ 각 caseFunc를 타입에 캐싱해야 함, 현재 오버헤드가 기존 방식보다 3초 길음
public class caseContainer : IEnumerable<caseBase> {
    // one warrior can have only one caseFocussing at once
    public effectFocussing thisCaseFocussing { get; private set; }
    private List<caseBase> listCaseBaseAll;
    private Dictionary<enumCaseType, List<caseBase>> dictCategory;

    public caseContainer() {
        thisCaseFocussing = null;
        listCaseBaseAll = new List<caseBase>();
        dictCategory = new Dictionary<enumCaseType, List<caseBase>>();
    }

    #region add_remove_clear
    public void addCase(caseBase parCase) {
        if (parCase == null) {
            return;
        }

        // one warrior can have only one caseFocussing at once
        if (parCase is effectFocussing tempCaseFocussing){
            if (thisCaseFocussing == null) {
                thisCaseFocussing = tempCaseFocussing;
            } else {
                return;
            }
        }

        listCaseBaseAll.Add(parCase);

        // store parCase in dictCategory
        if (!dictCategory.ContainsKey(parCase.caseType)) {
            dictCategory.Add(parCase.caseType, new List<caseBase>());
        }
        dictCategory[parCase.caseType].Add(parCase);
    }

    public void removeCase(caseBase parCase) {
        if (parCase == thisCaseFocussing) {
            thisCaseFocussing = null;
        }

        listCaseBaseAll.Remove(parCase);

        // remove parCase from dictCategory
        dictCategory[parCase.caseType]?.Remove(parCase);
    }

    public void clearCase() {
        foreach (caseBase cb in listCaseBaseAll) {
            removeCase(cb);
        }
    }
    #endregion add_remove_clear

    #region get
    public List<T> getCaseList<T>() {
        List<T> tempResult = new List<T>();

        foreach (caseBase cb in listCaseBaseAll) {
            if (cb is T tempT) {
                tempResult.Add(tempT);
            }
        }

        return tempResult;
    }

    public IEnumerable<caseBase> getCaseList(enumCaseType parCaseType) {
        if (dictCategory.ContainsKey(parCaseType)) {
            foreach (caseBase cb in dictCategory[parCaseType]) {
                yield return cb;
            }
        }
    }

    public int getCaseCount<T>() {
        int tempCount = 0;
        foreach (caseBase cb in listCaseBaseAll) {
            if (cb is T) {
                tempCount++;
            }
        }
        return tempCount;
    }

    public int getCaseCount(enumCaseType parCaseType) {
        int tempCount = 0;
        foreach (caseBase cb in listCaseBaseAll) {
            if (cb.caseType == parCaseType) {
                tempCount++;
            }
        }
        return tempCount;
    }
    #endregion get

    #region check
    public bool checkContainCaseType(caseBase parCase) {
        foreach (caseBase cb in listCaseBaseAll) {
            if (cb.GetType() == parCase.GetType()) {
                return true;
            }
        }
        return false;
    }

    public bool checkContainCaseConcrete(caseBase parCase) {
        return listCaseBaseAll.Contains(parCase);
    }
    #endregion check

    #region observe
    /*
        observeVoid<A> iterates ICase without any side-results
        A = type of ICase
    */
    public void observeVoid<A>(object[] parParameters, bool parIsSystemic = false) {
        // most obervation fails not during combat, please set parIsSystemic true for working regardless of combat-state
        if (combatManager.CM.combatState != enumCombatState.combat && !parIsSystemic) {
            return;
        }

        MethodInfo tempMethodInfo = typeof(A).GetMethod("caseFunc");
        if (tempMethodInfo == null) {
            Debug.Log("caseContianer.observeVoid error : " + typeof(A) + " doesn't have method \"caseFunc\"");
            return;
        }

        foreach (caseBase cb in listCaseBaseAll.ToArray()) {
            try {
                if (cb is A tempA) {
                    tempMethodInfo.Invoke(tempA, parParameters);
                }
            } catch (ArgumentException e) {
                StringBuilder tempSB = new StringBuilder("caseContainer.observeVoid error : " + cb.GetType().Name + " as " + typeof(A).Name + " got wrong parameters below\n");
                foreach (object obj in parParameters) {
                    tempSB.Append(obj.GetType().Name);
                    tempSB.Append(",\n");
                }
                Debug.Log(tempSB.ToString());
            }
        }
    }

    /*
        observeReturnEnumerate<A, B> iterates ICase and returns as type of B
        A = type of ICase, B = return type
    */
    public IEnumerable<B> observeReturnEnumerate<A, B>(object[] parParameters, bool parIsSystemic = false) {
        // most obervation fails not during combat, please set parIsSystemic true for working regardless of combat-state
        if (combatManager.CM.combatState != enumCombatState.combat && !parIsSystemic) {
            yield break;
        }

        MethodInfo tempMethodInfo = typeof(A).GetMethod("caseFunc");
        if (tempMethodInfo == null) {
            Debug.Log("caseContianer.observeReturnEnumerate error : " + typeof(A) + " doesn't have method \"caseFunc\"");
            yield break;
        }else if (tempMethodInfo.ReturnType != typeof(B)) {
            Debug.Log("caseContianer.observeReturnEnumerate error : intended return type - " + typeof(B).Name + " , actual method's return type - " + tempMethodInfo.ReturnType);
            yield break;
        }

        B tempResult = default;
        foreach (caseBase cb in listCaseBaseAll.ToArray()) {
            try {
                if (cb is A tempA) {
                    tempResult = (B)(tempMethodInfo.Invoke(tempA, parParameters));
                } else {
                    continue;
                }
            } catch (ArgumentException e) {
                StringBuilder tempSB = new StringBuilder("caseContainer.observeReturnEnumerate error : " + cb.GetType().Name + " as " + typeof(A).Name + " got wrong parameters below\n");
                foreach (object obj in parParameters) {
                    tempSB.Append(obj.GetType().Name);
                    tempSB.Append(",\n");
                }
                Debug.Log(tempSB.ToString());
                continue;
            }

            yield return tempResult;
        }
    }

    /*
        observeInterferable<A> iterates ICase and can be stopped by return of caseFunc
        A.caseFunc should return bool
        observeInterferable<A> doesn't invalify already-executed processes, you should insert the interfering caseBase in the earlier index for the intended result
        A = type of ICase, return = is interfere
    */
    public bool observeInterferable<A>(object[] parParameters, bool parIsSystemic = false) {
        return false;
        // most obervation fails not during combat, please set parIsSystemic true for working regardless of combat - state
        if (combatManager.CM.combatState != enumCombatState.combat && !parIsSystemic) {
            return false;
        }

        MethodInfo tempMethodInfo = typeof(A).GetMethod("caseFunc");
        bool tempIsInterfered = false;
        if (tempMethodInfo == null) {
            Debug.Log("caseContianer.observeInterferable error : " + typeof(A) + " doesn't have method \"caseFunc\"");
            return false;
        } else if (tempMethodInfo.ReturnType != typeof(bool)) {
            Debug.Log("caseContianer.observeInterferable error : intended return type - bool , actual method's return type - " + tempMethodInfo.ReturnType);
            return false;
        }

        foreach (caseBase cb in listCaseBaseAll.ToArray()) {
            try {
                if (cb is A tempA) {
                    tempIsInterfered = (bool)(tempMethodInfo.Invoke(tempA, parParameters));
                    // interfered iteration stops instantly, and it's expected that the returned processAbst also stops instantly and not be reenacted
                    if (tempIsInterfered) {
                        break;
                    }
                }
            } catch (ArgumentException e) {
                StringBuilder tempSB = new StringBuilder("caseContainer.observeInterferable error : " + cb.GetType().Name + " as " + typeof(A).Name + " got wrong parameters below\n");
                foreach (object obj in parParameters) {
                    tempSB.Append(obj.GetType().Name);
                    tempSB.Append(",\n");
                }
                Debug.Log(tempSB.ToString());
            }
        }

        return tempIsInterfered;
    }
    #endregion observe

    #region enumerate
    public IEnumerator<caseBase> GetEnumerator() {
        foreach (caseBase cb in listCaseBaseAll.ToArray()) {
            yield return cb;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return listCaseBaseAll.ToArray().GetEnumerator();
    }
    #endregion enumerate
}
