using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liner : MonoBehaviour {
    private carrierGeneric<line> carrierLine;

    public void Awake() {
        carrierLine = new carrierGeneric<line>(
            () => Instantiate(Resources.Load<GameObject>("Prefab/UI/LINE")).GetComponent<line>(),
            (x) => x.gameObject.SetActive(false)
        );
    }

    public line placeLine(RectTransform parTargetRectTransform, Vector3 parWorldPosition1, Vector3 parWorldPosition2) {
        line tempResult = carrierLine.getInterceptor();
        RectTransform tempLineRectTransform = tempResult.GetComponent<RectTransform>();
        Vector2 tempLocalPosition1 = parTargetRectTransform.convertVectorAcrossRect(parWorldPosition1);
        Vector2 tempLocalPosition2 = parTargetRectTransform.convertVectorAcrossRect(parWorldPosition2);
        Vector2 tempGap = tempLocalPosition2 - tempLocalPosition1;

        tempLineRectTransform.transform.SetParent(parTargetRectTransform.transform);
        tempLineRectTransform.localPosition = tempLocalPosition1;
        tempLineRectTransform.sizeDelta = new Vector2(tempGap.magnitude, tempLineRectTransform.sizeDelta.y);
        tempLineRectTransform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan(tempGap.y / tempGap.x) * Mathf.Rad2Deg);
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
