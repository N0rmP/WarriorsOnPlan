using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Cases;

public class showerCase : hoveredShowerAbst {
    protected static GameObject objCaseShown;

    public caseBase thisCase { get; protected set; }

    /*
    caseTypeShown restricts which case-type can be shown by this showerCase
    each case-type-enum will be AND operated with caseTypeShown and should result in non-zero to be shown
    */
    protected int caseTypeShown = 0;

    protected override void init() {
        setCase(null);
    }

    protected override GameObject makeGut() {
        // make similar-Singleton canvasCaseShown
        if (objCaseShown == null) {
            objCaseShown = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/UI/Gut/canvasCaseShown"));
            objCaseShown.transform.SetParent(gameManager.GM.canvasMain.transform);
            objCaseShown.SetActive(false);
        }

        return objCaseShown;
    }

    protected override bool doBeforeShow() {
        if (thisCase == null) {
            return false;
        }

        objGut.GetComponent<canvasCaseShown>().prepare(thisCase);
        return true;
    }

    public void setCase(caseBase parCase) {
        // showerCase ignores parCase if it's not the type to be shown on this shower
        if (parCase == null || (caseTypeShown | (int)parCase.caseType) == 0) {
            thisCase = null;
            return;
        } else { 
            thisCase = parCase;
        }

        doWhenSetCase();
    }

    public void setCaseTypeShown(enumCaseType[] parArray) {
        if (parArray == null) {
            caseTypeShown = 0b1111;
            return;
        }

        caseTypeShown = 0;
        foreach (enumCaseType ECT in parArray) {
            caseTypeShown = caseTypeShown | (int)ECT;
        }
    }

    public void setCaseTypeShown(int parCTS) {
        Math.Clamp(parCTS, 0b0001, 0b1111);
        caseTypeShown = parCTS;
    }

    // doWhenSetCase should cover the case when thisCase is null, recommended to use it like initiating the gut
    protected virtual void doWhenSetCase() { }
}
