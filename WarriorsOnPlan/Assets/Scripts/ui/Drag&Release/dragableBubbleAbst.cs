using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using Cases;

public abstract class dragableBubbleAbst : dragableObjectAbst {
    protected showerCase thisShower;

    protected caseBase thisTool_;

    public caseBase thisTool {
        get {
            return thisTool_;
        }
        set {
            if (value != null && value.caseType != enumCaseType.tool) {
                Debug.Log("error : non-tool caseBase is tried to be set in bubble");
                return;
            }
            thisTool_ = value;
            thisShower.setCase(value);
            setImage();
        }
    }

    public new void Awake() {
        base.Awake();
        thisShower = gameObject.AddComponent<showerCase>();
        thisShower.setCaseTypeShown(new enumCaseType[1] { enumCaseType.tool });
    }

    private void setImage() {
        transform.GetChild(0).GetComponent<Image>().sprite = thisTool_?.caseImage;
    }

    private void setImage(Sprite parSprite) {
        transform.GetChild(0).GetComponent<Image>().sprite = parSprite;

        if (parSprite == null) {
            gameObject.SetActive(false);
        }
    }
}
