using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumVFXType { 
    simple
}

public class fxComponent
{
    private Stack<GameObject> stackParticleSimple;

    private GameObject prefabSimple;

    public fxComponent() {
        prefabSimple = Resources.Load<GameObject>("Prefabs/VFX/vfxSimple");

        stackParticleSimple = new Stack<GameObject>();

        for (int i = 0; i < 5; i++) {
            stackParticleSimple.Push(UnityEngine.Object.Instantiate(prefabSimple));
        }
    }

    #region vfx
    public void callVFX(enumVFXType parEnumVFXType, enumMoveType parEnumMoveType, Vector3 parDeparture, Vector3 parDestination, float parTime = 1f) {
        vfxMovable tempVFXMovable = parEnumVFXType switch {
            enumVFXType.simple => getVFXSimple().GetComponent<vfxMovable>(),
            _ => null
        };
        Action<Vector3, float> tempDelegateMove = parEnumMoveType switch {
            enumMoveType.linear => tempVFXMovable.startLinearMove,
            enumMoveType.parabola => tempVFXMovable.startParabolaMove,
            _ => null
        };

        tempVFXMovable.transform.position = parDeparture;
        tempDelegateMove(parDestination, parTime);
        //¡Ú retrieve
    }

    private GameObject getVFXSimple() {
        return (stackParticleSimple.Count == 0) ? UnityEngine.Object.Instantiate(prefabSimple) : stackParticleSimple.Pop();
    }

    public void retrieve(vfxMovable parVFX) {
        Stack<GameObject> tempStack = null;
        switch (parVFX.thisVFXType) {
            case enumVFXType.simple:
                tempStack = stackParticleSimple;
                break;
            default:
                break;
        }

        tempStack.Push(parVFX.gameObject);
    }
    #endregion vfx
}
