using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public abstract class dragableBubbleAbst : dragableObjectAbst {
    protected caseBase thisTool_;

    public caseBase thisTool {
        get {
            return thisTool_;
        }
        set {
            if (value.caseType != enumCaseType.tool) {
                Debug.Log("error : non-tool caseBase is tried to be set in bubble");
                return;
            }
            thisTool_ = value;
            setImage(value.GetType().ToString());
        }
    }

    private void setImage(string parToolName) {
        if (!File.Exists(@".\Assets\Resources\Image\Tool\Image_" + parToolName + ".png")) {
            Debug.Log("file not found in bubble");
            parToolName = "weaponTester";
        }

        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Tool/Image_" + parToolName);
    }

    private void setImage(Sprite parSprite) {
        transform.GetChild(0).GetComponent<Image>().sprite = parSprite;

        if (parSprite == null) {
            gameObject.SetActive(false);
        }
    }
}
