using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// transparencyStripple works with Ocias' Stripple Transparency, please assure that all materials of the GameObject use it as shader
public class transparencyStripple : MonoBehaviour, ITransparency {
    private bool isWorking;
    private bool isAscending;
    private float destination;
    private float[] arrChangePerLoop;

    private List<Material> listMaterial;

    void Update() {
        if (isWorking) {
            for (int i = 0; i<listMaterial.Count; i++) {
                if ((!isAscending && listMaterial[i].GetFloat("_Transparency") >= destination) ||
                    (isAscending && listMaterial[i].GetFloat("_Transparency") <= destination)) {
                    listMaterial[i].SetFloat("_Transparency", listMaterial[i].GetFloat("_Transparency") + arrChangePerLoop[i]);
                }
            }
        }
    }

    public void init() {
        isWorking = false;
        listMaterial = gameObject.rakeMaterials();
        arrChangePerLoop = new float[listMaterial.Count];
    }

    public void fadeIn(float parTimer = 1f, float parDestination = 1f) {
        destination = parDestination;
        for (int i = 0; i < arrChangePerLoop.Length; i++) {
            arrChangePerLoop[i] = Mathf.Max(0, (parDestination - listMaterial[i].GetFloat("_Transparency")) / (counterPerSec.countPerSec * parTimer));
        }
        isAscending = true;
        isWorking = true;
    }

    public void fadeOut(float parTimer = 1f, float parDestination = 0f) {
        destination = parDestination;
        for (int i = 0; i < arrChangePerLoop.Length; i++) {
            arrChangePerLoop[i] = Mathf.Min(0, (parDestination - listMaterial[i].GetFloat("_Transparency")) / (counterPerSec.countPerSec) * parTimer);
        }
        isAscending = false;
        isWorking = true;
    }

    public void fadeStrict(float parValue) {
        isWorking = false;
        for (int i = 0; i < listMaterial.Count; i++) {
            listMaterial[i].SetFloat("_Transparency", parValue);
        }
    }
}
