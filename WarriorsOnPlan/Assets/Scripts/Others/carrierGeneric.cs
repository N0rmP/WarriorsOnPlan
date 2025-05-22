using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class carrierGeneric<T> {
    // hanger is °Ý³³°í... IM POOR AT ENGLISH SO I FORGOT IT ONCE SRY
    private Stack<T> hangerAvailable;
    private List<T> spaceOperating;

    private Func<T> delCreate;
    private Action<T> delReturn;

    public carrierGeneric(Func<T> parDelCreate, Action<T> pardelReturn = null) {
        hangerAvailable = new Stack<T>();
        spaceOperating = new List<T>();
        delCreate = parDelCreate;
        delReturn = pardelReturn ?? ((item) => { });
    }

    public T getInterceptor() {
        T tempT;
        tempT = (hangerAvailable.Count == 0) ? delCreate() : hangerAvailable.Pop();
        spaceOperating.Add(tempT);

        // if tempT is GameObject, activate it because it might be deactivated for optimizing
        if (tempT is GameObject tempObj) {
            tempObj.SetActive(true);
        }

        return tempT;
    }

    public void returnSingle(T parInterceptor) {
        // if spaceOperating doesn't contain parInterceptor, it might not be the intended T object (think of GameObject)
        if (!spaceOperating.Contains(parInterceptor)) {
            return;
        }

        // if parInterceptor is GameObject, deactivate it
        if (parInterceptor is GameObject tempObj) {
            tempObj.SetActive(false);
        }

        hangerAvailable.Push(parInterceptor);
        spaceOperating.Remove(parInterceptor);
        delReturn(parInterceptor);
    }

    public void returnTotal() {
        foreach (T item in spaceOperating.ToArray()) {
            returnSingle(item);
        }
    }

    public void destroySingle(T parInterceptor) {
        if (!spaceOperating.Contains(parInterceptor)) {
            return;
        }

        spaceOperating.Remove(parInterceptor);

        if (parInterceptor is GameObject tempObj) {
            GameObject.Destroy(tempObj);
        }
    }

    public void testCount() {
        Debug.Log("hangerAvailable : " + hangerAvailable.Count);
        Debug.Log("spaceOperating : " + spaceOperating.Count);
    }
}
