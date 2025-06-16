using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// canvasSandwitch shows uiActivatable by setting it as child object(A) of child object, (A) child object blocks accessing UI object below the uiActivatable
public class canvasSandwitch : MonoBehaviour {
    private static Stack<Transform> stackUiAlive = null;

    carrierGeneric<Transform> carrierCurtain;

    public void Awake() {
        if (stackUiAlive == null) {
            stackUiAlive = new Stack<Transform>();
        }

        carrierCurtain = new carrierGeneric<Transform>(
            () => {
                Transform tempTransform = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/UI/curtain")).transform;
                tempTransform.SetParent(transform);
                tempTransform.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
                tempTransform.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
                tempTransform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
                tempTransform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                tempTransform.GetComponent<Image>().enabled = false;
                return tempTransform;
            },
            (x) => {
                x.GetComponent<Image>().enabled = false;
            }
        );
    }

    #region stack_management
    public void pushUiActivatable(uiActivatable parUA) {
        if (stackUiAlive.Count == 0) {
            transform.SetAsLastSibling();
        }

        Transform tempTransformCurtain = carrierCurtain.getInterceptor();
        tempTransformCurtain.GetComponent<Image>().enabled = true;
        stackUiAlive.Push(tempTransformCurtain);

        parUA.transform.SetParent(tempTransformCurtain);
    }

    public uiActivatable popUiActivatable() {
        Transform tempTransformCurtain = stackUiAlive.Pop();
        tempTransformCurtain.GetComponent<Image>().enabled = false;

        uiActivatable tempReturn = tempTransformCurtain.GetChild(0).GetComponent<uiActivatable>();
        tempReturn.transform.SetParent(transform);
        
        carrierCurtain.returnSingle(tempTransformCurtain);
        return tempReturn;
    }

    public void clear() {
        while (stackUiAlive.Count > 0) {
            popUiActivatable();
        }
    }
    #endregion stack_management
}
