using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fxComponent
{
    private Stack<GameObject> stackParticleTest;

    private GameObject prefabTest;

    public fxComponent() {
        prefabTest = Resources.Load<GameObject>("/Prefabs/VFX/vfxSimple");

        stackParticleTest = new Stack<GameObject>();

        for (int i = 0; i < 5; i++) {
            stackParticleTest.Push(Object.Instantiate(prefabTest));
        }
    }

    #region vfx
    public void callTest() { 
        
    }
    #endregion vfx
}
