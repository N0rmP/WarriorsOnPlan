using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class exGameObject {
    public static bool checkHoveredWorld(this GameObject parObj) {
        Vector2 tempVec = Camera.main.WorldToScreenPoint(parObj.transform.position) - Input.mousePosition;
        return (
            Mathf.Abs(tempVec.x) <= gameManager.GM.option.stick &&
            Mathf.Abs(tempVec.y) <= gameManager.GM.option.stick * 0.86     // 0.87 is sqrt(3) / 2, the ratio of length when we see the object from 60 degree
            );
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
}
