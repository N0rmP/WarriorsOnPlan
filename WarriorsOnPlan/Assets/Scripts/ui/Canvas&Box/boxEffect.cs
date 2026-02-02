using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;
using System.Linq;

public class boxEffect : MonoBehaviour {
    private carrierGeneric<imgEffect> carrierIE;

    public void Awake() {
        GameObject tempIE = Resources.Load<GameObject>("Prefab/UI/imgRoundRectangle");
        carrierIE = new carrierGeneric<imgEffect>(
            () => {
                Debug.Log(tempIE + " : " + Instantiate(tempIE));
                return Instantiate(tempIE).AddComponent<imgEffect>();
            },
            (x) => {
                x.transform.SetParent(null);
            }
        );
    }

    public void openEffect(Thing parThing) {
        clear();

        if (parThing == null) {
            return;
        }

        foreach (caseBase cb in parThing.getCaseList(enumCaseType.effect)) {
            addEffect(cb);
        }
    }

    public void addEffect(caseBase parCase) {
        // ¡Ú currently no restriction on what to show on imgEffect, revise here when imgEffect is required to only show effect
        if (parCase == null) {
            return;
        }

        imgEffect tempIE = carrierIE.getInterceptor();
        tempIE.setCase(parCase);
        tempIE.transform.SetParent(transform);
        tempIE.transform.localScale = Vector3.one;
    }

    public void removeEffect(caseBase parCasee) {
        imgEffect tempIE;
        foreach (Transform tr in transform) {
            if (tr.TryGetComponent<imgEffect>(out tempIE) && tempIE.thisCase == parCasee) {
                carrierIE.returnSingle(tempIE);
                return;
            }
        }
    }

    public void clear() {
        carrierIE.returnTotal();
    }
}
