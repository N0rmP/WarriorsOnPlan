using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sizerScrollContentLayoutGroup : MonoBehaviour {
    public void Start() {
        if (!(TryGetComponent<LayoutGroup>(out _) && TryGetComponent<RectTransform>(out _))) {
            Destroy(this);
        }
    }

    public void OnTransformChildrenChanged() {
        RectTransform tempRT = GetComponent<RectTransform>();
        tempRT.sizeDelta = new Vector2(
            Mathf.Max(LayoutUtility.GetPreferredWidth(tempRT), transform.parent.GetComponent<RectTransform>().sizeDelta.x),
            Mathf.Max(LayoutUtility.GetPreferredHeight(tempRT), transform.parent.GetComponent<RectTransform>().sizeDelta.x)
        );
    }
}
