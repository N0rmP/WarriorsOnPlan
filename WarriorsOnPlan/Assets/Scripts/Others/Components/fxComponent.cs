using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fxComponent
{
    private Stack<GameObject> stackParticleTest;

    private GameObject prefabTest;

    public fxComponent() {
        prefabTest = Resources.Load<GameObject>("Prefabs/VFX/vfxSimple");

        stackParticleTest = new Stack<GameObject>();

        for (int i = 0; i < 5; i++) {
            stackParticleTest.Push(Object.Instantiate(prefabTest));
        }
    }

    #region vfx
    public void callTest(Vector3 parDeparture, Vector3 parDestination, float parTime = 1f) {
        GameObject tempObj = (stackParticleTest.Count == 0) ? Object.Instantiate(prefabTest) : stackParticleTest.Pop();

        tempObj.transform.position = parDeparture;
        combatManager.CM.MC.addParabolaer(tempObj, parDestination, parTime);
        tempObj.GetComponent<vfxMovable>().delReturn = this.retrieveTest;
    }

    public void retrieveTest(GameObject parObj) {
        stackParticleTest.Push(parObj);
    }
    #endregion vfx
}
