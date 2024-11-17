using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public enum EDirection {
    //direction     = index //required calculation for direction-to-coordinates conversion (directionConverter)
    //you can find that z calculation is two indice latter than x calculation
    forward         = 0,    //0     1
    forward_right   = 1,    //1     1
    right           = 2,    //1     0
    backward_right  = 3,    //1     -1
    backward        = 4,    //0     -1
    backward_left   = 5,    //-1    -1
    left            = 6,    //-1    0
    forward_left    = 7,    //-1    1
    none            = 8     // this EDirection represents no direction, used for initiation of EDirectionToDeparture when graph search
}

public class node : MonoBehaviour {
    private static readonly int[] directionConverter = new int[8] { 0, 1, 1, 1, 0, -1, -1, -1 };
    public int coor0 { get; private set; }
    public int coor1 { get; private set; }
    public bool isPlrPlacable { get; private set; }

    public node[] link { get; private set; }
    public Thing thingHere { get; private set; }

    // variables below is used for various searches, it's declared public being used only by external instances
    public bool swissArmyVisited;
    public EDirection swissArmyEDirection;

    public void Start() {
        // Plr can place warriors here if node is in half of nodes near Plr
        isPlrPlacable = (coor1 < combatManager.CM.GC.size1 / 2);
        if (isPlrPlacable) {
            returnColor();
            gameObject.AddComponent<releasableNode>().init(this);
        }
    }

    public node init(int parCoor0, int parCoor1) {
        coor0 = parCoor0;
        coor1 = parCoor1;
        swissArmyVisited = false;
        link = new node[8];

        transform.position = getVector3();

        return this;
    }

    // it's fxcking super faster than accessing transform
    // ... i think it's fxcking super rare case, anyway if node itself moves you should change this method too
    public Vector3 getVector3() {
        return new Vector3(coor0 * 2, 0f, coor1 * 2);
    }

    /*
    public (int c0, int c1) getPosition() {
        return (coor0 * 2, coor1 * 2);
    }*/

    /*
    public (int c0, int c1) getlink_Coordinates(EDirection parEDir) {
        return (
            coor0 + directionConverter[(int)parEDir],
            coor1 + directionConverter[((int)parEDir + 6) % 8]
        );
    }*/

    public static int getDistance(node n1, node n2) {
        return (Mathf.Abs(n1.coor0 - n2.coor0) > Mathf.Abs(n1.coor1 - n2.coor1)) ? Mathf.Abs(n1.coor0 - n2.coor0) : Mathf.Abs(n1.coor1 - n2.coor1);
    }

    // setLink set not only this node's link_ but also the target node's link_
    public void setLink(node parNode, EDirection parDir) {
        link[(int)parDir] = parNode;
        parNode.link[((int)parDir + 4) % 8] = this;
    }

    #region thingManagement
    public void placeThing(Thing parThing) {
        if (thingHere != null) { return; /*★사용자 정의 exception 만들어서 "node에 뭐 있는데요?" UI라도 띄우기*/ }

        parThing.curPosition = this;
        this.thingHere = parThing;
        parThing.gameObject.transform.position = getVector3();
    }

    public bool sendThing(EDirection parDir) {
        //check if its not boundary, and if there is something on destination
        if ((link[(int)parDir] == null) && (link[(int)parDir].thingHere != null)) {
            return false;
        }
        thingHere.curPosition = link[(int)parDir];
        link[(int)parDir].thingHere = thingHere;
        this.thingHere = null;
        return true;
    }

    public bool sendThing(node parNode, bool parIsTeleport = false) {
        //check if its not boundary, and if there is something on destination
        if ((parNode == null) || (parNode.thingHere != null)) {
            return false;
        }
        thingHere.curPosition = parNode;
        parNode.thingHere = thingHere;
        this.thingHere = null;

        if (parIsTeleport) {
            parNode.thingHere.transform.position = parNode.getVector3();
        }

        return true;
    }
    #endregion thingManagement

    public void setColor(Color parColor) {
        GetComponent<SpriteRenderer>().color = parColor;
    }

    public void returnColor() {
        GetComponent<SpriteRenderer>().color = 
            (combatManager.CM.combatState != enumCombatState.preparing) ? new Color(1f, 1f, 1f, 1f) : 
            (isPlrPlacable ? new Color(0.5f, 1f, 0.5f, 1f) : new Color(1f, 1f, 1f, 1f));
    }
}
