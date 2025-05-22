using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

using Cases;

public class releasableInventory : releasableObjectAbst {
    private Transform contentTransform;

    private GameObject objCurtainInventory;

    private carrierGeneric<GameObject> carrierBubble;

    public void Awake() {
        contentTransform = transform.GetChild(0).GetChild(0);

        objCurtainInventory = transform.GetChild(1).gameObject;
        objCurtainInventory.SetActive(false);

        carrierBubble = new carrierGeneric<GameObject>(
            () => {
                GameObject tempResult = Instantiate(combatUIManager.CUM.prefabBubble);
                tempResult.AddComponent<dragableBubbleInventory>();
                return tempResult;
            },
            (x) => {
                x.transform.SetParent(null);
            }
            );
    }

    protected override bool doWhenReleased(object[] parParameters) {
        if (combatUIManager.CUM.CStatus.thisThing == null || !combatManager.CM.checkControllability(combatUIManager.CUM.CStatus.thisThing)) {
            return false;
        }

        caseBase tempTool = (caseBase)parParameters[0];

        // check if parParameters[0] is tool, it's required because it depends on enumCaseType not just Type
        if (((caseBase)parParameters[0]).caseType != enumCaseType.tool) {
            Debug.Log("releasableInventory error : non-tool caseBase is in bubble");
            return false;
        }

        // if the tool is already contained in the warrior, skip it
        if (combatUIManager.CUM.CStatus.thisThing.checkContainConcreteCase(tempTool)){
            return false;
        }

        // adding only one more bubble is more efficient than updating total bubbles, but who knows that later onAdd-destroying-other-tool is created
        combatUIManager.CUM.CStatus.thisThing.addCase((caseBase)parParameters[0]);

        // ★ inventory 이미지 갱신, 기능 대부분을 구현한 뒤 필요한지 재고
        return true;
    }

    public void openInventory(Thing parThing) {
        clear();
        addTool(parThing.getCaseList(enumCaseType.tool, false).ToArray());
    }

    public void setInteractivity(bool parIsControllable) {
        objCurtainInventory.SetActive(!parIsControllable);
    }

    #region addNremove
    public void addTool(caseBase parTool) {
        if (parTool.caseType != enumCaseType.tool || combatUIManager.CUM.CStatus.thisThing == null) {
            return;
        }

        if (!combatUIManager.CUM.CStatus.thisThing.checkContainConcreteCase(parTool)) {
            combatUIManager.CUM.CStatus.thisThing.addCase(parTool);
        }

        GameObject tempBubble = carrierBubble.getInterceptor();
        tempBubble.GetComponent<dragableBubbleInventory>().thisTool = parTool;
        tempBubble.transform.SetParent(contentTransform);
    }

    public void addTool(caseBase[] parArr) {
        foreach (caseBase cb in parArr) {
            addTool(cb);
        }
    }

    public void removeBubble(dragableBubbleInventory parBubble, bool isUnequip) {
        carrierBubble.returnSingle(parBubble.gameObject);

        caseBase tempTool = parBubble.thisTool;
        if (isUnequip) {
            combatUIManager.CUM.TS.addBubble(tempTool);
            // removed tool should be also removed from owner regardless of who called (bubble, releasable-curtain, owner etc.)
            if (combatUIManager.CUM.CStatus.thisThing.checkContainConcreteCase(tempTool)) {
                combatUIManager.CUM.CStatus.thisThing.removeCase(tempTool);
            }
        }
    }

    public void removeTool(caseBase parTool) {
        dragableBubbleInventory tempBubble;
        foreach (Transform t in contentTransform) {
            if (t.TryGetComponent<dragableBubbleInventory>(out tempBubble) && tempBubble.thisTool == parTool) {
                removeBubble(tempBubble, false);
            }
        }
    }

    public void clear(bool isUnequip = false) {
        dragableBubbleInventory tempBubble;
        for (int i = contentTransform.childCount; i > 0; i--) {
            if (contentTransform.GetChild(i - 1).TryGetComponent<dragableBubbleInventory>(out tempBubble)) {
                removeBubble(tempBubble, isUnequip);
            }
        }
    }
    #endregion addNremove
}
