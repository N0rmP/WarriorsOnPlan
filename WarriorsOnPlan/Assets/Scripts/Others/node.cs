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
    public IPlacableOccupier occupierHere { get; private set; }
    private List<IPlacableSharer> listSharerHere;

    // variables below is used for various searches, it's declared public being used only by external instances
    public bool swissArmyVisited;
    public EDirection swissArmyEDirection;

    public void Start() {
        gameObject.AddComponent<releasableNode>().init(this);
        GetComponent<releasableNode>().enabled = false;
    }

    public node init(int parCoor0, int parCoor1) {
        coor0 = parCoor0;
        coor1 = parCoor1;
        swissArmyVisited = false;
        link = new node[8];
        listSharerHere = new List<IPlacableSharer>();

        transform.position = getVector3();

        return this;
    }

    public Vector3 getVector3() {
        return new Vector3(coor0 * 2, 0f, coor1 * 2);
    }

    public (int c0, int c1) getCoor() {
        return (coor0 , coor1);
    }

    // setLink set not only this node's link_ but also the target node's link_
    public void setLink(node parNode, EDirection parDir) {
        link[(int)parDir] = parNode;
        parNode.link[((int)parDir + 4) % 8] = this;
    }

    #region place
    public bool placeHere(IPlacableOccupier parOccupier, bool parIsTeleport = false) {
        if (occupierHere != null || parOccupier == null) { 
            return false;
        }
        
        parOccupier.curPosition = this;
        occupierHere = parOccupier;
        if (parIsTeleport) {
            parOccupier.setPosition(getVector3());
        }

        return true;
    }

    public bool placeHere(IPlacableSharer parSharer, bool parIsTeleport = false) {
        if (parSharer == null || listSharerHere.Contains(parSharer)) {
            return false;
        }

        parSharer.curPosition = this;
        listSharerHere.Add(parSharer);
        if (parIsTeleport) {
            parSharer.setPosition(getVector3());
        }

        return true;
    }

    public bool placeHere(IPlacable parPlacable, bool parIsTeleport = false) {
        switch (parPlacable) {
            case IPlacableOccupier tempIPO:
                return placeHere(tempIPO, parIsTeleport);
            case IPlacableSharer tempIPS:
                return placeHere(tempIPS, parIsTeleport);
            default:
                return false;
        }
    }
    #endregion place

    #region expel
    // expelHere only make occupierHere not to be on this node, animation ain't included
    public void expelHere(bool parIsPositionChange = true) {
        if (occupierHere == null) {
            return;
        }

        occupierHere.curPosition = null;
        if (parIsPositionChange) {
            occupierHere.setPosition(new Vector3(50f, 0f, 50f));
        }
        occupierHere = null;
    }

    public void expelHere(IPlacableSharer parSharer, bool parIsPositionChange = true) {
        if (parSharer == null) {
            return;
        }

        parSharer.curPosition = null;
        if (parIsPositionChange) {
            parSharer.setPosition(new Vector3(50f, 0f, 50f));
        }
        listSharerHere.Remove(parSharer);
    }
    #endregion expel

    #region send
    /*
    public bool sendThere(EDirection parDir) {
        //check if its not boundary, and if there is something on destination
        if ((link[(int)parDir] == null) && (link[(int)parDir].occupierHere != null)) {
            return false;
        }

        occupierHere.curPosition = link[(int)parDir];
        link[(int)parDir].occupierHere = occupierHere;
        this.occupierHere = null;

        return true;
    }
    */

    public bool sendThere(node there, bool parIsTeleport = false) {
        if (there == null || there.occupierHere != null) {
            return false;
        }

        if (!there.placeHere(occupierHere, parIsTeleport)) {
            return false;
        }
        this.occupierHere = null;

        return true;
    }

    public bool sendThere(IPlacableSharer parSharer, node there, bool parIsTeleport = false) {
        // sendSharer checks invalidity same as sendOccupier, and also if parSharer is contained here and not there
        if (there == null || 
            parSharer == null || 
            !listSharerHere.Contains(parSharer) || 
            there.listSharerHere.Contains(parSharer)) {
            return false;
        }

        if (!there.placeHere(parSharer, parIsTeleport)) {
            return false;
        }
        this.listSharerHere.Remove(parSharer);

        return true;
    }
    #endregion send

    #region swap
    // swapThing basically sends thing to parNode, if some-Thing is on parNode then change positions of two
    public bool swapOccupier(node parNode, bool parIsTeleport = false) {
        if (parNode == null || parNode == this) {
            return false;
        }

        IPlacableOccupier tempOccupierBuffer = parNode.occupierHere;
        parNode.expelHere(false);
        sendThere(parNode, parIsTeleport);
        placeHere(tempOccupierBuffer, parIsTeleport);

        return true;
    }
    #endregion swap    

    #region paint
    public void setColor(Color parColor) {
        GetComponent<SpriteRenderer>().color = parColor;
    }

    public void autoColor() {
        GetComponent<SpriteRenderer>().color = 
            (combatManager.CM.combatState != enumCombatState.preparing) ? new Color(1f, 1f, 1f, 0.25f) : 
            (isPlrPlacable ? new Color(0.5f, 1f, 0.5f, 1f) : new Color(1f, 1f, 1f, 1f));
    }
    #endregion paint

    public override string ToString() {
        return "(" + coor0 + "," + coor1 + ")";
    }

    public void setIsPlrPlacable(Placablers.IPlacabler parPlacabler) {
        isPlrPlacable = parPlacabler.checkPlacable(this);
        GetComponent<releasableNode>().enabled = isPlrPlacable;
        autoColor();
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

    // getDirectionClosestSorted returns collection of 8 EDirection in cloest order to parVectorToDestination
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
