using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class exGameObject {
    // checkHoveredWorld technically doesn't check actual hovering, it checks if mouse hovers in the radius of approximately 1 around this GameObject
    public static bool checkHoveredWorld(this GameObject parObj) {
        Vector2 tempVec = (Camera.main.WorldToScreenPoint(parObj.transform.position) - Input.mousePosition) / (Vector2)gameManager.GM.canvasMain.transform.localScale;
        return (
            Mathf.Abs(tempVec.x) <= gameManager.GM.option.stick &&
            Mathf.Abs(tempVec.y) <= gameManager.GM.option.stickDegreed
            );
    }

    public static Vector2 getCanvasMainLocalPosition(this GameObject parObj) {
        // Debug.Log("getCanvasMainLocalPosition : " + parObj + " / " + ((Vector2)(Camera.main.WorldToScreenPoint(parObj.transform.position)) / gameManager.GM.canvasMain.transform.localScale - gameManager.GM.canvasMain.GetComponent<RectTransform>().sizeDelta * 0.5f));
        return (Vector2)(Camera.main.WorldToScreenPoint(parObj.transform.position)) / gameManager.GM.canvasMain.transform.localScale - gameManager.GM.canvasMain.GetComponent<RectTransform>().sizeDelta * 0.5f;
    }

    public static Bounds getTotalBounds(this GameObject parObj) {
        Renderer[] tempMR = parObj.transform.GetComponentsInChildren<Renderer>();
        Bounds tempResult = new Bounds();

        if (tempMR.Length > 0 && tempMR[0].enabled) {
            tempResult = tempMR[0].bounds;

            if (tempMR.Length > 1) {
                for(int i=1; i < tempMR.Length; i++) {
                    if (tempMR[i].enabled) {
                        tempResult.Encapsulate(tempMR[i].bounds);
                    }
                }
            }
        }

        return tempResult;        
    }

    public static List<Material> rakeMaterials(this GameObject parObj) {
        Renderer tempRenderer;
        List<Material> tempListMaterial = new List<Material>();

        void tempRakeMaterials(Transform parT) {
            foreach (Transform tt in parT) {
                if (tt.gameObject.activeSelf && tt.TryGetComponent<Renderer>(out tempRenderer)) {
                    tempListMaterial.AddRange(tempRenderer.materials);
                }
                tempRakeMaterials(tt);
            }
        }

        tempRakeMaterials(parObj.transform);

        return tempListMaterial;
    }

    // FindThoroughly only finds object from active scene regardless of its isActive
    public static GameObject FindThoroughly(this GameObject parObj, string parName) {
        GameObject tempResult = null;

        // FindThorouglySmall only searches parObj's children, you should check parObj on your own before starting the first recursive call
        void FindThoroughlySmall(GameObject parObj) {
            if (parObj.name == parName) {
                tempResult = parObj;
            } else {
                foreach (Transform tr in parObj.transform) {
                    FindThoroughlySmall(tr.gameObject);
                    if (tempResult != null) {
                        break;
                    }
                }
            }
        }
        
        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects()) {
            FindThoroughlySmall(obj);
            if (tempResult != null) {
                break;
            }
        }

        return tempResult;
    }
}
