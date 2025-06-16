using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class exRectTransform {
    public static bool checkHovered(this RectTransform parRT) {
        Vector3 tempMin = parRT.position - new Vector3(parRT.rect.width * parRT.pivot.x, parRT.rect.height * parRT.pivot.y, 0f);
        Vector3 tempMax = parRT.position + new Vector3(parRT.rect.width * (1.0f - parRT.pivot.x), parRT.rect.height * (1.0f - parRT.pivot.y), 0f);

        Vector3 tempPosMouse = Input.mousePosition;

        return (
            (tempPosMouse.x >= tempMin.x) &&
            (tempPosMouse.x <= tempMax.x) &&
            (tempPosMouse.y >= tempMin.y) &&
            (tempPosMouse.y <= tempMax.y)
            );
    }

    // convertVectorAcrossRect only works with Canvas.Screen Space - Overlay, please implement another method if not
    public static Vector2 convertVectorAcrossRect(this RectTransform parDestination, Vector3 parWorldPosition) {
        Vector2 tempResult;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parDestination,
            parWorldPosition,
            null,
            out tempResult
        );
        return tempResult;

    }
}
