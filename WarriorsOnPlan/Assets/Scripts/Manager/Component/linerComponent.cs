using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linerComponent {
    private carrierGeneric<line> carrierLine;

    public linerComponent() {
        carrierLine = new carrierGeneric<line>(
            () => GameObject.Instantiate(Resources.Load<GameObject>("Prefab/UI/LINE")).GetComponent<line>(),
            (x) => x.gameObject.SetActive(false)
        );
    }

    public line placeLine(RectTransform parTargetRectTransform, Vector3 parLoaclPosition1, Vector3 parLoaclPosition2) {
        line tempResult = carrierLine.getInterceptor();
        RectTransform tempLineRectTransform = tempResult.GetComponent<RectTransform>();
        Vector2 tempGap = parLoaclPosition2 - parLoaclPosition1;

        tempLineRectTransform.SetParent(parTargetRectTransform);
        tempLineRectTransform.localScale = Vector3.one;
        tempLineRectTransform.localPosition = parLoaclPosition1;
        tempLineRectTransform.sizeDelta = new Vector2(tempGap.magnitude, gameManager.GM.option.stick * 0.15f);
        float tempTheta = Mathf.Acos(tempGap.x / (tempGap.magnitude == 0 ? 0.0001f : tempGap.magnitude)) * Mathf.Rad2Deg;
        tempLineRectTransform.rotation = Quaternion.Euler(0f, 0f,
             tempGap.y < 0 ? 360 - tempTheta : tempTheta
        );
        tempLineRectTransform.gameObject.SetActive(true);

        return tempResult;
    }

    public void retrieveLine(line tempLine) {
        carrierLine.returnSingle(tempLine);
    }

    public void clear() {
        carrierLine.returnTotal();
    }
}
