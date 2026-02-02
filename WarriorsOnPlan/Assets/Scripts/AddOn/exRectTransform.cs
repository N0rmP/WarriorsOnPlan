using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class exRectTransform {
    public static bool checkHovered(this RectTransform parRT) {
        Vector2 tempMin = parRT.getCanvasMainLocalPosition(parRT.rect.min);
        Vector3 tempMax = parRT.getCanvasMainLocalPosition(parRT.rect.max);
        Vector2 tempPosMouse = (Vector2)(Input.mousePosition) / gameManager.GM.canvasMain.transform.localScale - gameManager.GM.canvasMain.GetComponent<RectTransform>().sizeDelta * 0.5f;

        return (
            (tempPosMouse.x >= tempMin.x) &&
            (tempPosMouse.x <= tempMax.x) &&
            (tempPosMouse.y >= tempMin.y) &&
            (tempPosMouse.y <= tempMax.y)
        );
    }

    public static Vector2 convertAcrossRect(this RectTransform parDeparture, RectTransform parDestination, Vector3 parLocalLocalPosition) {
        // cannot convert between two objects from different scenes
        if (parDeparture.gameObject.scene != parDestination.gameObject.scene) {
            return Vector2.zero;
        }

        // instantly return parLocalLocalPosition if parDeparture & parDestination are same
        if (parDeparture == parDestination) {
            return parLocalLocalPosition;
        }

        /*
        // departure가 destination의 부모일 때
        parDestination에서부터 parLocalLocalPosition -= curRectTransform 반복 (curRectTransform == parDeparture 직전까지)
        // departure가 destination의 자식일 때
        parDeparture에서부터 parLocalLocalPosition += curRectTransform 반복 (curRectTransform == parDestination 직전까지)
        // 둘 사이의 부모 자식 관계가 직속으로 존재하지 않을 때
            // 둘 상의 공통 부모 tempLeastCommonParent 찾기
            // 먼저 parLocalLocalPosition += curRectTransform 반복 (curRectTransform == tempLeastCommonParent 직전까지)
            // 이후 parLocalLocalPosition -= curRectTransform 반복 (curRectTransform == parDestination 직후까지)
        */

        Vector3 tempResult = parLocalLocalPosition;
        // be aware not to confuse tempDeparture/tempDestination and parDeparture/parDestination
        Transform tempCurTransform;
        Transform tempDeparture;
        Transform tempDestination;
        int tempInterpolater = 1;
        if (parDeparture.IsChildOf(parDestination)) {
            tempDeparture = parDeparture;
            tempDestination = parDestination;
            tempInterpolater = 1;
        } else if (parDestination.IsChildOf(parDeparture)) {
            tempDeparture = parDestination;
            tempDestination = parDeparture;
            tempInterpolater = -1;
        } else {
            // rake all parents of parDeparture
            List<Transform> tempListDepartureParent = new List<Transform>();
            tempCurTransform = parDeparture;
            while (tempCurTransform.parent != null) {
                tempListDepartureParent.Add(tempCurTransform.parent);
                tempCurTransform = tempCurTransform.parent;
            }
            // find Least-Common-Parent
            RectTransform tempVia = (RectTransform)(gameManager.GM.canvasMain.transform);
            tempCurTransform = parDestination;
            while (tempCurTransform.parent != null) {
                tempCurTransform = tempCurTransform.parent;
                if (tempListDepartureParent.Contains(tempCurTransform)) {
                    tempVia = (RectTransform)tempCurTransform;
                    break;
                }
            }
            // find LocalLocalPosition via tempVia
            return tempVia.convertAcrossRect(parDestination, 
                parDeparture.convertAcrossRect(tempVia, parLocalLocalPosition)
            );
        }

        // calculations all consist of adding, so the adding-order doesn't matter and only be done from child to parent
        tempCurTransform = tempDeparture;
        while (tempCurTransform != tempDestination) {
            tempResult += tempCurTransform.localPosition * tempInterpolater;
            tempCurTransform = tempCurTransform.parent;
        }
        return tempResult;
    }

    // parLocalPosition is localPosition on parRectTransform whose canvasMain-localPosition you want to know
    // you can use another overloaded getCanvasMainLocalPosition below to find just parRectTransform's pivot's canvasMain-localPosition
    public static Vector2 getCanvasMainLocalPosition(this RectTransform parRectTransform, Vector3 parLocalPosition) {
        RectTransform tempCurRectTransform = parRectTransform;
        Vector3 tempLocalPosition = parLocalPosition;
        while (tempCurRectTransform.parent != null) {
            tempLocalPosition += tempCurRectTransform.localPosition;
            tempCurRectTransform = (RectTransform)tempCurRectTransform.parent;
        }
        
        return new Vector2(tempLocalPosition.x, tempLocalPosition.y);
    }

    public static Vector2 getCanvasMainLocalPosition(this RectTransform parRectTransform) {
        return getCanvasMainLocalPosition(parRectTransform, Vector3.zero);
    }

    // getLocalLocalPosition converts localPosition in canvasMain to localPosition in parRectTransform
    public static Vector2 getLocalLocalPosition(this RectTransform parRectTransform, Vector3 parCanvasMainLocalPosition) {
        RectTransform tempCurRectTransform = parRectTransform;
        // getLocalLocalPosition is done in reverse of getCanvasMainLocalPosition
        // and the procedures all consist of adding, the adding-order doesn't matter
        while (tempCurRectTransform.parent != null) {
            parCanvasMainLocalPosition -= tempCurRectTransform.localPosition;
            tempCurRectTransform = (RectTransform)tempCurRectTransform.parent;
        }

        return new Vector2(parCanvasMainLocalPosition.x, parCanvasMainLocalPosition.y);
    }

    public static void resizeToChildSize(this RectTransform parRectTransform, int parChildIndex = 0) {
        parRectTransform.GetChild(parChildIndex).GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
        parRectTransform.GetChild(parChildIndex).GetComponent<ContentSizeFitter>().SetLayoutVertical();
        parRectTransform.sizeDelta = parRectTransform.GetChild(parChildIndex).GetComponent<RectTransform>().sizeDelta;
    }
}
