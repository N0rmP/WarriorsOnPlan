using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;

public class boxUpgrade : MonoBehaviour {
    private List<GameObject> listUpgradeCell;

    public void Awake() {
        listUpgradeCell = new List<GameObject>();
    }

    public void prepareBoxUpgrade(upgradeAbst[] parArrUpgradeActive) {
        GameObject tempPrefab = Resources.Load<GameObject>("Prefab/UI/imgRoundRectangle");
        carrierCrude.updateCarrierCrude<GameObject, upgradeAbst>(
            listUpgradeCell.ToArray(),
            parArrUpgradeActive,
            () => {
                GameObject tempReturn = Instantiate(tempPrefab, transform);
                tempReturn.AddComponent<showerCase>().setCaseTypeShown(new enumCaseType[1] { enumCaseType.upgrade });
                listUpgradeCell.Add(tempReturn);
                return tempReturn;
            },
            (a, b) => {
                a.GetComponent<imgRoundRectangle>().setCase(b);
                a.GetComponent<showerCase>().setCase(b);
                a.SetActive(true);
            },
            (a) => {
                a.SetActive(false);
            }
        );
    }
}
