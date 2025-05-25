using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        // Plr can place warriors on half of total nodes near Plr
        isPlrPlacable = (coor1 < combatManager.CM.GC.size1 / 2);
        if (isPlrPlacable) {
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

    public Vector3 getVector3() {
        return new Vector3(coor0 * 2, 0f, coor1 * 2);
    }

    public (int c0, int c1) getCoor() {
        return (coor0 , coor1);
    }

    /*
    public (int c0, int c1) getlink_Coordinates(EDirection parEDir) {
        return (
            coor0 + directionConverter[(int)parEDir],
            coor1 + directionConverter[((int)parEDir + 6) % 8]
        );
    }*/

    // setLink set not only this node's link_ but also the target node's link_
    public void setLink(node parNode, EDirection parDir) {
        link[(int)parDir] = parNode;
        parNode.link[((int)parDir + 4) % 8] = this;
    }

    #region thingManagement
    public void placeThing(Thing parThing) {
        if (thingHere != null) { return; /* ★사용자 정의 exception 만들어서 "node에 뭐 있는데요?" UI라도 띄우기 */ }

        parThing.curPosition = this;
        this.thingHere = parThing;
        parThing.setPosition(getVector3());
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
            parNode.thingHere.setPosition(parNode.getVector3());
        }

        return true;
    }

    // expelThing only make thingHere not to be on this node, other processes like animation or position change ain't included
    public void expelThing(bool parIsPositionChange = true) {
        if (thingHere == null) {
            Debug.Log("node (" + coor0 + " , " + coor1 + ") tried to expel thingHere while there's nothing on it");
            return;
        }

        thingHere.curPosition = null;
        if (parIsPositionChange) {
            thingHere.setPosition(new Vector3(50f, 0f, 50f));
        }
        thingHere = null;        
    }
    #endregion thingManagement

    public void setColor(Color parColor) {
        GetComponent<SpriteRenderer>().color = parColor;
    }

    public void autoColor() {
        GetComponent<SpriteRenderer>().color = 
            (combatManager.CM.combatState != enumCombatState.preparing) ? new Color(1f, 1f, 1f, 0.25f) : 
            (isPlrPlacable ? new Color(0.5f, 1f, 0.5f, 1f) : new Color(1f, 1f, 1f, 1f));
    }

    public override string ToString() {
        return "(" + coor0 + "," + coor1 + ")";
    }

    #region StaticMethods
    // getDistance returns only the larger one between each coordinates difference, it's for distance calculation in weapon (or skill) range
    public static int getDistance(node n1, node n2) {
        return Math.Max(
                Math.Abs(n1.coor0 - n2.coor0),
                Math.Abs(n1.coor1 - n2.coor1)
            );
        // return (Mathf.Abs(n1.coor0 - n2.coor0) > Mathf.Abs(n1.coor1 - n2.coor1)) ? Mathf.Abs(n1.coor0 - n2.coor0) : Mathf.Abs(n1.coor1 - n2.coor1);
    }

    // getTechnicalDistance returns actual float distance between two nodes, it's used for in-script calculation like selecterClosest
    public static float getTechnicalDistance(node n1, node n2) {
        return (n2.getVector3() - n1.getVector3()).magnitude;
    }

    // getLikestDirection returns the direction with the most similar vector with (target.vector - source.vector)
    public static EDirection getLikestDirection(node source, node target) {
        Vector2 tempComparing = (new Vector2(target.coor0 - source.coor0, target.coor1 - source.coor1)).normalized;
        int tempResult = 0;
        float tempMinDistance = float.MaxValue;
        Vector2 tempComparedCur;

        for (int i = 0; i < 8; i++) {
            tempComparedCur = (new Vector2(directionConverter[i], directionConverter[(i + 2) % 8])).normalized;
            if ((tempComparedCur - tempComparing).magnitude < tempMinDistance) {
                tempMinDistance = (tempComparedCur - tempComparing).magnitude;
                tempResult = i;
            }
        }

        return (EDirection)tempResult;
    }

    public static IEnumerable<EDirection> getDirectionClosestSorted(Vector2 parVectorToDestination) {
        List<(EDirection edir, float dist)> tempPriorityList = new List<(EDirection edir, float dist)>();
        float tempCurDistance;

        for (int i = 0; i < 8; i++) {
            tempCurDistance = (parVectorToDestination - new Vector2(directionConverter[i], directionConverter[(i + 2) % 8]).normalized).magnitude;
            if (i == 0) {
                tempPriorityList.Add(((EDirection)i, tempCurDistance));
                continue;
            }

            for (int j = 0; j < i; j++) {
                if (tempPriorityList[j].dist > tempCurDistance) {
                    tempPriorityList.Insert(j, ((EDirection)i, tempCurDistance));
                    break;
                } else if (j == i - 1){
                    tempPriorityList.Add(((EDirection)i, tempCurDistance));
                    break;
                }
            }
        }

        return from tup in tempPriorityList select tup.edir;
    }
    #endregion StaticMethods

    #region test
    public void testLinks() {
        StringBuilder tempSB = new StringBuilder(" - - - - - ( " + coor0 + " , " + coor1 + " ) node link test - - - - - \n");
        node tempLink;
        for (int i=0; i<8; i++) {
            tempLink = link[i];
            tempSB.Append((EDirection)i + " : ( ");
            tempSB.Append(tempLink?.coor0);
            tempSB.Append(" , ");
            tempSB.Append(tempLink?.coor1);
            tempSB.Append(" )\n");
        }

        Debug.Log(tempSB.ToString());
    }
    #endregion test
}
