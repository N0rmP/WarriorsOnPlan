using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showerCase : hoveredShowerAbst {
    protected static GameObject objCaseShown;

    protected caseBase thisCase { get; set; }

    protected override void init() {
        // make similar-Singleton canvasCaseShown
        if (objCaseShown == null) {
            objCaseShown = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Gut/canvasCaseShown"));
            objCaseShown.transform.SetParent(gameManager.GM.canvasMain.transform);
            objCaseShown.SetActive(false);
        }

        objGut = objCaseShown;
    }

    protected override bool doBeforeShow() {
        if (thisCase == null) {
            return false;
        }

        objGut.GetComponent<canvasCaseShown>().prepare(thisCase);
        return true;
    }

    public virtual void setCase(caseBase parCase) {
        thisCase = parCase;
    }
}
