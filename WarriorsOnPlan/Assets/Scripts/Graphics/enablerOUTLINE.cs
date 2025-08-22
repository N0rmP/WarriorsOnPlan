using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enablerOUTLINE : MonoBehaviour {
    List<Material> listMaterialTotal;

    public void Awake() {
        listMaterialTotal = gameObject.rakeMaterials();
    }

    public void enableOUTLINE() {
        float tempOutlineVolume = gameManager.GM.option.stick * 0.15f;
        foreach (Material mat in listMaterialTotal) {
            mat.SetFloat("_OutlineVolume", tempOutlineVolume);
            mat.EnableKeyword("OUTLINE");
        }
    }

    public void disableOUTLINE() {
        foreach (Material mat in listMaterialTotal) {
            mat.DisableKeyword("OUTLINE");
        }
    }

    public void setColor(Color parColor) {
        foreach (Material mat in listMaterialTotal) {
            mat.SetColor("_OutlineColor", parColor);
        }
    }
}
