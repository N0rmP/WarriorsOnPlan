using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class exGameObject {
    public static bool checkHoveredWorld(this GameObject parObj) {
        return (gameManager.GM.cameraMain.WorldToScreenPoint(parObj.transform.position) - Input.mousePosition).magnitude <= gameManager.GM.option.stick;
    }
}
