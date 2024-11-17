using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uiTextShower : uiHoveredShowerAbst
{
    //objTextShower is basic HoveredShower with two text (Name / Description)
    private static GameObject objTextShower = null;

    protected string strName;
    protected string strDescription;

    protected override void init() {
        // make similar-Singleton Info
        if (objTextShower == null) {
            objTextShower = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/boxTextShower"));
            objTextShower.transform.SetParent(
                // parent transform is not gameObject.transform but canvas.transform (as possible) to assure cooperating with mouse position trouble-lessly
                transform.parent
                );
            deshow();
        }
    }

    public void initText(string parName, string parDescription) {
        strName = parName;
        strDescription = parDescription;
    }

    protected override void show() {
        objTextShower.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = strName;
        objTextShower.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = strDescription;

        interpolatePivot(objTextShower);
        moveToMouse(objTextShower);

        objTextShower.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
        objTextShower.SetActive(true);
    }

    protected override void deshow() {
        objTextShower.SetActive(false);
    }
}
