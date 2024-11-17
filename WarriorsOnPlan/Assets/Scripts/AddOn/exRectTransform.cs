using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class exRectTransform {
    public static bool checkHovered(this RectTransform parRT) {
        Vector3 tempMin = parRT.position - new Vector3(parRT.rect.width * parRT.pivot.x, parRT.rect.height * parRT.pivot.y, 0f);
        Vector3 tempMax = parRT.position + new Vector3(parRT.rect.width * (1.0f - parRT.pivot.x), parRT.rect.height * (1.0f - parRT.pivot.y), 0f);

        Vector3 posMouse = Input.mousePosition;

        return (
            (posMouse.x >= tempMin.x) &&
            (posMouse.x <= tempMax.x) &&
            (posMouse.y >= tempMin.y) &&
            (posMouse.y <= tempMax.y)
            );
    }
}
