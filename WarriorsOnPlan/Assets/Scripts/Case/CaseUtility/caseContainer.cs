using Cases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class caseContainer : IEnumerable<caseBase> {
    // one warrior can have only one caseFocussing at once
    public caseFocussing thisCaseFocussing { get; private set; }
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
        if (parCase is caseFocussing tempCaseFocussing){
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
            if (cb is T tempCA) {
                tempResult.Add(tempCA);
            }
        }

        return tempResult;
    }

    public List<caseBase> getCaseList(enumCaseType parCaseType) {
        List<caseBase> tempResult = dictCategory.ContainsKey(parCaseType) ? dictCategory[parCaseType] : new List<caseBase> { };

        return tempResult;
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
        foreach (A cb in getCaseList<A>()) {
            try {
                tempMethodInfo.Invoke(cb, parParameters);
            } catch (ArgumentException e) {
                string temp = cb.GetType().Name + " as " + typeof(A).Name + " : failed to observe due to parameter error \n parameters : ";
                foreach (object obj in parParameters) {
                    temp += obj.GetType().Name + " , ";
                }
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


        if (tempMethodInfo.ReturnType != typeof(B)) {
            Debug.Log("innature type incorrespondence : intended return type - " + typeof(B).Name + " , actual method's return type - " + tempMethodInfo.ReturnType);
            yield break;
        }

        B tempResult;
        foreach (A cb in getCaseList<A>()) {
            try {
                tempResult = (B)tempMethodInfo.Invoke(cb, parParameters);
            } catch (ArgumentException e) {
                string temp = cb.GetType().Name + " as " + typeof(A).Name + " : failed to observe due to parameter error \n parameters : ";
                foreach (object obj in parParameters) {
                    temp += obj.GetType().Name + " , ";
                }
                Debug.Log(temp);
                continue;
            } catch (InvalidConversionException e) {
                Debug.Log("type incorrespondenc during iteration : intended return type - " + typeof(B).Name + " , actual method's return type - " + tempMethodInfo.ReturnType);
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
        // most obervation fails not during combat, please set parIsSystemic true for working regardless of combat - state
        if (combatManager.CM.combatState != enumCombatState.combat && !parIsSystemic) {
            return false;
        }

        MethodInfo tempMethodInfo = typeof(A).GetMethod("caseFunc");
        // interfered process observing & process be stopped instantly and not recorded for reenacting
        bool tempIsInterfered = false;

        if (tempMethodInfo.ReturnType != typeof(bool)) {
            Debug.Log("innature type incorrespondence : intended return type - bool , actual method's return type - " + tempMethodInfo.ReturnType);
            return tempIsInterfered;
        }
        
        foreach (A cb in getCaseList<A>()) {
            try {
                tempIsInterfered = (bool)tempMethodInfo.Invoke(cb, parParameters);
                // interfered iteration stops instantly, and it's expected that the returned processAbst also stops instantly and not be reenacted
                if (tempIsInterfered) {
                    break;
                }
            } catch (ArgumentException e) {
                string temp = cb.GetType().Name + " as " + typeof(A).Name + " : failed to observe due to parameter error \n parameters : ";
                foreach (object obj in parParameters) {
                    temp += obj.GetType().Name + " , ";
                }
                Debug.Log(temp);
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
