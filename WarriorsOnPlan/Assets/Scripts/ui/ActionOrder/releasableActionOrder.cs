using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class releasableActionOrder : releasableObjectAbst {
    private static int curTotalReleasableActionOrder = 0;
    public int thisActionOrder { get; private set; } = -9999;
    public Thing thisThing { get; private set; }
    private line thisLine_ = null;
    public line thisLine {
        get {
            return thisLine_;
        }
        set {
            if (thisLine_ == null) {
                thisLine_ = value;
            }
        }
    }

    public void Awake() {
        curTotalReleasableActionOrder++;
        setNumber(curTotalReleasableActionOrder);
        targetEnumDrag = (int)(enumDrag.thingOriginal | enumDrag.thingActionOrder);
    }

    private void setNumber(int parActionOrder) {
        if (thisActionOrder != -9999) {
            return;
        }

        thisActionOrder = parActionOrder;
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = parActionOrder.ToString();
    }

    #region thing_management
    public bool assignThing(Thing parThing) {
        if (parThing == null) {
            thisThing = null;
            transform.GetChild(0).GetComponent<Image>().sprite = null;
            return false;
        }

        // return if any RAO has parThing as thisThing, swapThing works well because it sets thisThing before swapping begins
        if (combatUIManager.CUM.SAO.findRAO(parThing) is not null) {
            return false;
        }

        thisThing = parThing;
        transform.GetChild(0).GetComponent<Image>().sprite = thisThing.portrait;
        transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

        combatManager.CM.HouC.changeActionOrder(thisThing, thisActionOrder);
        return true;
    }

    public void swapThing(releasableActionOrder parRAO) {
        Thing tempThingBufferSource = thisThing;
        Thing tempThingBufferTarget = parRAO.thisThing;
        thisThing = null;
        
        if (parRAO.assignThing(tempThingBufferSource)) {
            combatUIManager.CUM.SAO.arrangeLineSingle(parRAO);
        }else{
            Debug.Log("releasableActionOrder " + thisActionOrder + " , " + parRAO.thisActionOrder + " tried to swap, but " + parRAO.thisActionOrder + ".assignActionOrder failed");
        }

        if (assignThing(tempThingBufferTarget)) {
            combatUIManager.CUM.SAO.arrangeLineSingle(this);
        } else{
            Debug.Log("releasableActionOrder " + thisActionOrder + " , " + parRAO.thisActionOrder + " tried to swap, but " + thisActionOrder + ".assignActionOrder failed");
        }
    }
    #endregion thing_management    

    #region line_management
    public void showLine() {
        thisLine?.gameObject.SetActive(true);
    }

    public void hideLine() {
        thisLine?.gameObject.SetActive(false);
    }

    public void retrieveLine() {
        combatUIManager.CUM.SAO.GetComponent<liner>().retrieveLine(thisLine);
        thisLine_ = null;
    }
    #endregion line_management

    protected override bool doWhenReleased(enumDrag parCurDragging, object[] parParameters) {
        if (!combatManager.CM.checkControllability((Thing)parParameters[0])) {
            return false;
        }
        
        releasableActionOrder tempRAO2Swaop = parCurDragging switch {
            enumDrag.thingOriginal => combatUIManager.CUM.SAO.findRAO(thisThing),
            enumDrag.thingActionOrder => (releasableActionOrder)parParameters[1],
            _ => null
        };      

        swapThing(tempRAO2Swaop);

        return false;
    }
}
