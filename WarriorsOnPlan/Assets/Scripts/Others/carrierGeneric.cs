using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// carrierGeneric supports object-pooling
public class carrierGeneric<T> {
    // hanger is °Ý³³°í... IM POOR AT ENGLISH SO I FORGOT IT ONCE SRY
    private Stack<T> hangerAvailable;
    private List<T> spaceOperating;

    private Func<T> delCreate;
    private Action<T> delReturn;

    /*
        parDelCreate : delegate creating new pooled-object
        parDelReturn : delegate retrieving pooled-object that runs out of its use
    */
    public carrierGeneric(Func<T> parDelCreate, Action<T> parDelReturn = null) {
        hangerAvailable = new Stack<T>();
        spaceOperating = new List<T>();
        delCreate = parDelCreate;
        delReturn = parDelReturn ?? ((item) => { });
    }

    public T getInterceptor() {
        T tempT;
        tempT = (hangerAvailable.Count == 0) ? delCreate() : hangerAvailable.Pop();
        spaceOperating.Add(tempT);

        if (tempT is GameObject tempObj) {
            tempObj.SetActive(true);
        }

        return tempT;
    }

    public void returnSingle(T parInterceptor) {
        if (!spaceOperating.Contains(parInterceptor)) {
            return;
        }

        if (delReturn != null) {
            delReturn(parInterceptor);
        }

        if (parInterceptor is GameObject tempObj) {
            tempObj.SetActive(false);
        }

        hangerAvailable.Push(parInterceptor);
        spaceOperating.Remove(parInterceptor);        
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

    #region test
    public void testCount() {
        Debug.Log("hangerAvailable : " + hangerAvailable.Count);
        Debug.Log("spaceOperating : " + spaceOperating.Count);
    }
    #endregion test
}
