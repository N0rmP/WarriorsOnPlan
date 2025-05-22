using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class showerText : hoveredShowerAbst {
    //objTextShown is basic HoveredShower with two text (Name, Description)
    private static GameObject objTextShown = null;

    protected string strName;
    protected string strDescription;

    protected override GameObject makeGut() {
        // make similar-Singleton canvasTextShown
        if (objTextShown == null) {
            objTextShown = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/UI/Gut/canvasTextShown"));
            objTextShown.transform.SetParent(gameManager.GM.canvasMain.transform);
            objTextShown.SetActive(false);
        }

        return objTextShown;
    }

    public void initText(string parName, string parDescription) {
        strName = parName;
        strDescription = parDescription;
    }

    protected override bool doBeforeShow() {
        objTextShown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = strName;
        objTextShown.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = strDescription;
        return true;
    }
}
