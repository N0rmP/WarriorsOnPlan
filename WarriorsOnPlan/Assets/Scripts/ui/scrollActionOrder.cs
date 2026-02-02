using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrollActionOrder : MonoBehaviour {
    private List<releasableActionOrder> listRAO;

    private GameObject prefabRAO;

    private Transform transformContent;

    public void Awake() {
        listRAO = new List<releasableActionOrder>();
        prefabRAO = Resources.Load<GameObject>("Prefab/UI/releasableActionOrderBlock");
        transformContent = transform.GetChild(0).GetChild(0);
    }

    // prepareBoxActionOrderBelt prepares listRAO referencing houseComponent, be aware not to call this before houseComponent initiation
    public void prepareBoxActionOrderBelt() {
        Thing[] tempThingPlayer = combatManager.CM.HouC.getArrActionOrder(enumSide.player);
        
        for (int i = 0; i< Math.Max(listRAO.Count, tempThingPlayer.Length); i++) {
            // hide excessed RAO / make lacked RAO from listRAO
            if (i >= tempThingPlayer.Length) {
                listRAO[i].assignThing(null);
                listRAO[i].gameObject.SetActive(false);
                continue;
            } else if (i >= listRAO.Count) {
                listRAO.Add(makeRAO());
            }

            // set things on RAO
            listRAO[i].assignThing(tempThingPlayer[i]);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(transformContent.GetComponent<RectTransform>());
        arrangeLineTotal();
    }

    // makeRAO makes new releasableActionOrder
    private releasableActionOrder makeRAO() {
        releasableActionOrder tempResult = GameObject.Instantiate(prefabRAO, transformContent).GetComponent<releasableActionOrder>();
        return tempResult;
    }

    public void confirm() {
        List<Thing> tempListThing = new List<Thing>();
        foreach (releasableActionOrder rao in listRAO) {
            tempListThing.Add(rao.thisThing);
        }

        combatManager.CM.HouC.rearrangePlayerActionOrder(tempListThing);
    }

    public releasableActionOrder findRAO(Thing parThingLookingFor) {
        foreach (releasableActionOrder rao in listRAO) {
            if (rao.isActiveAndEnabled && rao.thisThing == parThingLookingFor) {
                return rao;
            }
        }
        return null;
    }

    #region line
    public void arrangeLineSingle(releasableActionOrder parRAO) {
        // scrollActionOrder object can be inactive for tutorial
        // first of all placing line with ActionOrder works only during preparing
        // do not place line with unenabled RAO
        if (!isActiveAndEnabled || combatManager.CM.combatState != enumCombatState.preparing || !parRAO.isActiveAndEnabled) {
            return;
        }

        // clear parRAO.thisLine if it's not null
        if (parRAO.thisLine != null) {
            parRAO.retrieveLine();
        }

        parRAO.thisLine = gameManager.GM.LC.placeLine(
            (RectTransform)transform.parent,
            parRAO.GetComponent<RectTransform>().convertAcrossRect((RectTransform)transform.parent, new Vector3(0f, 70f, 0f)),
            gameManager.GM.canvasMain.GetComponent<RectTransform>().convertAcrossRect(
                (RectTransform)transform.parent,
                parRAO.thisThing.gameObject.getCanvasMainLocalPosition()
            )
        );
    }

    public void arrangeLineTotal() {
        foreach (releasableActionOrder rao in listRAO) {
            if (rao.isActiveAndEnabled) {
                arrangeLineSingle(rao);
            } else {
                break;
            }
        }
    }    

    public void arrangeLineSingle(Thing parThing) {
        arrangeLineSingle(findRAO(parThing));
    }

    public void clearLineTotal() {
        foreach (releasableActionOrder rao in listRAO) {
            rao.retrieveLine();
        }
    }
    #endregion line

    
}
